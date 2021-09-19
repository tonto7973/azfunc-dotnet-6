using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace test_func_6.Auth;

public class AccessTokenAuthorizationHandler : IAuthorizationHandler
{
    private readonly IOptionsMonitor<AccessTokenOptions> _options;

    public AccessTokenAuthorizationHandler(IOptionsMonitor<AccessTokenOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var accessTokenIdentity = context.User?.Identities?
            .FirstOrDefault(i => i.AuthenticationType == TokenValidationParameters.DefaultAuthenticationType
                              && i.HasClaim("iss", _options.CurrentValue.Authority));
        if (accessTokenIdentity != null)
        {
            context.Succeed(context.Requirements.First());
        }
        return Task.CompletedTask;
    }
}
