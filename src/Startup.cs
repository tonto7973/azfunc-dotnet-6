using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using test_func_6.Auth;

[assembly: FunctionsStartup(typeof(test_func_6.Startup))]

namespace test_func_6
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAccessTokenAuthorization();
            builder.Services.AddOptions<AccessTokenOptions>()
                .Configure(options => {
                    options.Authority = "https://api-token-translation.services.craneware.com";
                    options.Audience = "trisus";
                });
        }
    }
}
