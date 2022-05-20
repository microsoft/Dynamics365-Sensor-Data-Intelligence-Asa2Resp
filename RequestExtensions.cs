// SAMPLE CODE NOTICE
// THIS SAMPLE CODE IS MADE AVAILABLE AS IS. MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
// OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
// THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
// NO TECHNICAL SUPPORT IS PROVIDED. YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AzureStreamAnalyticsToRedisFunction
{
    internal static class RequestExtensions
    {
        /// <summary>
        /// Read the body of the request as a string.
        /// </summary>
        public static async Task<string> ReadRequestBodyAsStringAsync(this HttpRequest request)
        {
            using var reader = new StreamReader(request.Body);

            return await reader.ReadToEndAsync();
        }
    }
}
