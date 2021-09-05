using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using test_func_6.Auth;

namespace test_func_6
{
    public class HttpExample
    {
        [FunctionName("HttpExample")]
        [FunctionAuthorize(Role = "CwAdmin")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequest req,
            ClaimsPrincipal claimsPrincipal,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string responseMessage = "HTTP triggered function: " + name + " executed successfully for user "
                + claimsPrincipal.Identity.Name;

            return new OkObjectResult(responseMessage);
        }
    }
}
