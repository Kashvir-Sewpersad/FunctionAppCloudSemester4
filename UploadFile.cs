﻿using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FunctionAppCloudSemester4
{
    internal class UploadFile
    {
        public static class UploadFile1
        {
            [Function("UploadFile")]
            public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
                ILogger log)
            {
                string shareName = req.Query["shareName"];
                string fileName = req.Query["fileName"];

                if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
                {
                    return new BadRequestObjectResult("Share name and file name must be provided.");
                }

                var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
                var shareServiceClient = new ShareServiceClient(connectionString);
                var shareClient = shareServiceClient.GetShareClient(shareName);
                await shareClient.CreateIfNotExistsAsync();
                var directoryClient = shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient(fileName);

                using var stream = req.Body;
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);

                return new OkObjectResult("File uploaded to Azure Files");
            }
        }
    }
}
