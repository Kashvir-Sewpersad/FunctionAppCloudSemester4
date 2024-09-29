using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FunctionAppCloudSemester4
{
    internal class UploadBlob
    {
        public static class UploadBlob1
        {
            [Function("UploadBlob")]
            public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
                ILogger log)
            {
                string containerName = req.Query["containerName"];
                string blobName = req.Query["blobName"];

                if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName))
                {
                    return new BadRequestObjectResult("Container name and blob name must be provided.");
                }

                var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();
                var blobClient = containerClient.GetBlobClient(blobName);

                using var stream = req.Body;
                await blobClient.UploadAsync(stream, true);

                return new OkObjectResult("Blob uploaded");
            }
        }
    }
}

