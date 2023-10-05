// SAMPLE CODE NOTICE
// THIS SAMPLE CODE IS MADE AVAILABLE AS IS. MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
// OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
// THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
// NO TECHNICAL SUPPORT IS PROVIDED. YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace AzureStreamAnalyticsToRedisFunction
{
    public static class AzureStreamAnalyticsToRedis
    {
        private static readonly IConnectionMultiplexer redis;

        static AzureStreamAnalyticsToRedis()
        {
            string redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString", EnvironmentVariableTarget.Process);

            redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        [FunctionName("AzureStreamAnalyticsToRedis")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = (await req.ReadRequestBodyAsStringAsync())?.Trim();

            if (string.IsNullOrEmpty(requestBody))
            {
                // Azure Stream Analytics connectivity request
                return new NoContentResult();
            }

            if (!requestBody.StartsWith("[") || !requestBody.EndsWith("]"))
            {
                string error = "Received metrics are not a JSON-array";

                log.LogError(error);

                return new BadRequestObjectResult(new { error });
            }

            JArray metricPoints = JArray.Parse(requestBody);

            IDatabase db = redis.GetDatabase();

            long ignoreMetricPointsBeforeUts = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(6)).ToUnixTimeMilliseconds();

            foreach (JObject metricPoint in metricPoints)
            {
                if (!(metricPoint.TryGetValue("MetricKey", StringComparison.OrdinalIgnoreCase, out JToken metricKey) && metricKey.Type == JTokenType.String))
                {
                    log.LogError($"Missing string MetricKey property for {metricPoint}");
                    continue;
                }
                if (!(metricPoint.TryGetValue("Uts", StringComparison.OrdinalIgnoreCase, out JToken uts) && uts.Type == JTokenType.Integer))
                {
                    log.LogError($"Missing unix timestamp (uts) property for {metricPoint}");
                    continue;
                }

                string redisKey = metricKey.Value<string>();
                string redisValue = JsonConvert.SerializeObject(metricPoint);

                long metricUtsValue = uts.Value<long>();
                if (metricUtsValue < ignoreMetricPointsBeforeUts)
                {
                    // skip to avoid unnecessary CPU-cycles in Redis
                    log.LogInformation($"Skipping metric point with key ${redisKey} as its uts value is too far in the past");
                    continue;
                })

                await db.SortedSetAddAsync(redisKey, redisValue, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }

            return new OkResult();
        }
    }
}
