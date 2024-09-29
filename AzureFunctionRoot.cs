﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FunctionAppCloudSemester4
{
    internal class AzureFunctionRoot
    {

        public class AzureFunctionRoot1
        {
            private readonly ILogger<AzureFunctionRoot> _logger;

            public AzureFunctionRoot1(ILogger<AzureFunctionRoot> logger)
            {
                _logger = logger;
            }

            [Function("HTTPTest")]
            public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new OkObjectResult("Welcome to Azure Functions!");
            }
        }
    }
}
