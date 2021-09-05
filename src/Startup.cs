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
                    options.Authority = "https://micah.okta.com/oauth2/aus2yrcz7aMrmDAKZ1t7";
                    options.Audience = "okta-oidc-fun";
                    options.NameClaimType = "sub";
                    options.RoleClaimType = "groups";
                });
        }
    }
}
