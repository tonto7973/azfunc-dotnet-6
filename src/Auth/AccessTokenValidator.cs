using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace test_func_6.Auth;

internal class AccessTokenValidator
{
    private readonly JwtBearerOptions _options;

    internal AccessTokenValidator(JwtBearerOptions options)
    {
        _options = options;
    }

    public async Task<ClaimsPrincipal> ValidateAsync(AuthenticationHeaderValue authenticationHeader)
    {
        try
        {
            return Validate(authenticationHeader.Parameter);
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            _options.ConfigurationManager.RequestRefresh();
            await UpdateSigningKeysAsync();
            return Validate(authenticationHeader.Parameter);
        }
    }

    private ClaimsPrincipal Validate(string token)
        => _options.SecurityTokenValidators
            .First()
            .ValidateToken(token, _options.TokenValidationParameters, out _);

    private async Task UpdateSigningKeysAsync()
    {
        var config = await _options.ConfigurationManager.GetConfigurationAsync(default);
        _options.TokenValidationParameters.IssuerSigningKeys = config.SigningKeys;
    }
}
