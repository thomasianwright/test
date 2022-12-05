using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Property.WebApp.BusinessLogic.ApiClients;
using Property.WebApp.BusinessLogic.Providers;

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
        var dateCompare = AccessTokenExpiry.HasValue &&
                          DateTime.Compare(AccessTokenExpiry.Value.DateTime, DateTime.Now) > 0;
        var accessTokenValid = dateCompare && !string.IsNullOrEmpty(AccessToken);

        Console.WriteLine($"Access token valid: {accessTokenValid}");
        
        if (!accessTokenValid && IsLoggedIn)
        {
            await RefreshTokenAsync();
        }

        return AccessToken ?? string.Empty;
    }

    public Task Logout()
    {
        AccessToken = null;
        RefreshToken = null;
        UserId = null;
        AccessTokenExpiry = null;
        IsLoggedIn = false;
        RefreshTokenExpiry = null;
        
        return Task.CompletedTask;
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
    
    private async Task RefreshTokenAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        AccessToken = string.Empty;
        AccessTokenExpiry = null;

        var propertyApiClient = scope.ServiceProvider.GetRequiredService<IPropertyApiClient>();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        if (string.IsNullOrEmpty(RefreshToken)) return;

        var response = await propertyApiClient.RefreshTokenAsync(new RegenerateTokenDto
        {
            Token = RefreshToken,
            UserId = UserId
        });

        AccessToken = response.Token;
        AccessTokenExpiry = response.Expiry;

        var user = await localStorageService.GetItemAsync<AuthenticateResponseDto>(JwtAuthenticationStateProvider.AuthStateStorageName);
        user.Token = response.Token;
        user.TokenExpiry = response.Expiry;

        await localStorageService.SetItemAsync(JwtAuthenticationStateProvider.AuthStateStorageName, user);
    }
}