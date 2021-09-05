using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace test_func_6.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAccessTokenAuthorization(this IServiceCollection services)
        {
            new AuthenticationBuilder(services)
                .AddJwtBearer();

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(serviceProvider =>
                new PostConfigureOptions<JwtBearerOptions>(null, options =>
                {
                    var accessTokenOptionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<AccessTokenOptions>>();
                    var accessTokenOptions = accessTokenOptionsMonitor.CurrentValue;
                    var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                        accessTokenOptions.Authority + "/.well-known/openid-configuration",
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever { RequireHttps = true }
                    );
                    var tokenValidator = new AccessTokenValidator(options);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.InboundClaimTypeMap.Clear();
                    options.TokenValidationParameters.Configure(accessTokenOptions);
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(tokenHandler);
                    options.ConfigurationManager = configurationManager;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async (context) =>
                        {
                            var parsed = AuthenticationHeaderValue.TryParse(context.Request.Headers["Authorization"], out var authHeader);
                            if (parsed)
                            {
                                try
                                {
                                    context.Principal = await tokenValidator.ValidateAsync(authHeader);
                                    context.Success();
                                }
                                catch (SecurityTokenException ex)
                                {
                                    context.Fail(ex);
                                }
                            }
                        }
                    };
                })
            );
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthLevelUser", p =>
                {
                    p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireAssertion(_ => true);
                });
            });
            services.AddSingleton<IAuthorizationHandler, AccessTokenAuthorizationHandler>();

            return services;
        }

        private static void Configure(this TokenValidationParameters validationParameters, AccessTokenOptions options)
        {
            validationParameters.NameClaimType = "unique_name";
            validationParameters.RoleClaimType = "role";
            validationParameters.RequireSignedTokens = true;
            validationParameters.ValidAudience = options.Audience;
            validationParameters.ValidateAudience = true;
            validationParameters.ValidIssuer = options.Authority;
            validationParameters.ValidateIssuer = true;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateLifetime = true;
        }
    }
}
