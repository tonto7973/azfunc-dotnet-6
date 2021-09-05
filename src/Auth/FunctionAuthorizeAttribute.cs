using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;

namespace test_func_6.Auth
{
    public class FunctionAuthorizeAttribute : FunctionInvocationFilterAttribute
    {
        public string Role { get; set; }

        public override async Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var request = (HttpRequest)executingContext.Arguments["req"];
            var principal = request.HttpContext.User;
            var identity = (ClaimsIdentity)principal.Identity;
            if (Role != null && !principal.IsInRole(Role))
            {
                var response = request.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                await response.CompleteAsync();

                throw new UnauthorizedAccessException("User not authorized to access function " + executingContext.FunctionName);
            }
        }
    }
}
