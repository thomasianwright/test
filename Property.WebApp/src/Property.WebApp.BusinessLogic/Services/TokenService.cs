using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Caliqon.Property.WebApp.BusinessLogic.Services;

public class TokenService : ITokenService
{
    private readonly IServiceProvider _serviceProvider;

    public TokenService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        AccessToken = string.Empty;
    }

    private string? AccessToken { get; set; }
    private string? RefreshToken { get; set; }
    public string? UserId { get; private set; }
    private DateTimeOffset? AccessTokenExpiry { get; set; }
    private DateTimeOffset? RefreshTokenExpiry { get; set; }
    private bool IsLoggedIn { get; set; }
    

    public async Task<string> GetTokenAsync()
    {
        var accessTokenValid = AccessTokenExpiry.HasValue && AccessTokenExpiry.Value.ToUniversalTime() < DateTime.Now.ToUniversalTime();
        Console.WriteLine($"Access token valid: {accessTokenValid}");
        if(accessTokenValid && IsLoggedIn)
        {
            return await RefreshTokenAsync();
        }
        
        return AccessToken ?? string.Empty;
    }

    private async Task<string> RefreshTokenAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var propertyApiClient = scope.ServiceProvider.GetRequiredService<IPropertyApiClient>();

        if (string.IsNullOrEmpty(RefreshToken)) return string.Empty;
        
        var response = await propertyApiClient.RefreshTokenAsync(new RegenerateTokenDto
        {
            Token = RefreshToken,
            UserId = UserId
        });

        AccessToken = response.Token;
        AccessTokenExpiry = response.Expiry;

        return AccessToken;
    }

    public void SetAuthenticationState(AuthenticateResponseDto authenticateResponseDto)
    {
        Console.WriteLine(JsonConvert.SerializeObject(authenticateResponseDto));
        AccessToken = authenticateResponseDto.Token;
        RefreshToken = authenticateResponseDto.RefreshToken;
        UserId = authenticateResponseDto.UserId;
        AccessTokenExpiry = authenticateResponseDto.TokenExpiry;
        RefreshTokenExpiry = authenticateResponseDto.RefreshTokenExpiry;
        IsLoggedIn = true;
    }
}