# Azure Function: Azure Stream Analytics to RESP (Redis)

This repository contains code for an Azure Function to proxy metric key updates from HTTP calls
to the RESP protocol (Redis).

The primary purpose of the Azure Function is to enable Redis key updates from Azure Stream Analytics jobs and is referenced by the
[microsoft/Dynamics365-Sensor-Data-Intelligence-ARMDeployments](https://github.com/microsoft/Dynamics365-Sensor-Data-Intelligence-ARMDeployments) sample repository
for Dynamics 365 SCM Sensor Data Intelligence.

If an [Azure Stream Analytics job outputs to this Azure Function](https://docs.microsoft.com/azure/stream-analytics/azure-functions-output),
there must always be a **`metricKey`** column (case insensitive) such that the function knows which Redis key to upsert.

> This sample code is made available as is. Microsoft makes no warranties, whether express or implied, of fitness for a particular purpose, of accuracy or completeness of responses, of results or conditions of merchantability.
> The entire risk of the use or the results from the use of this sample template remains with the user.
> No technical support is provided beyond what is mentioned in [`SUPPORT.md`](./SUPPORT.md).

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
