using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
        // semaphore = new SemaphoreSlim(1, 1);
    }

    private string? AccessToken { get; set; }
    private string? RefreshToken { get; set; }
    public string? UserId { get; private set; }
    private DateTimeOffset? AccessTokenExpiry { get; set; }
    private DateTimeOffset? RefreshTokenExpiry { get; set; }
    private bool IsLoggedIn { get; set; }
    // private readonly SemaphoreSlim semaphore;


    public async Task<string> GetTokenAsync()
    {
        // await semaphore.WaitAsync();
        // Console.WriteLine(JsonConvert.SerializeObject(new
        // {
        //     AccessToken,
        //     RefreshToken,
        //     UserId,
        //     AccessTokenExpiry,
        //     IsLoggedIn,
        //     RefreshTokenExpiry
        // }));
        // try
        // {
        var dateCompare = AccessTokenExpiry.HasValue &&
                          DateTime.Compare(AccessTokenExpiry.Value.DateTime, DateTime.Now) > 0;
        var accessTokenValid = dateCompare && !string.IsNullOrEmpty(AccessToken);

        Console.WriteLine($"Access token valid: {accessTokenValid}");
        if (!accessTokenValid && IsLoggedIn)
        {
            return await RefreshTokenAsync();
        }

        return AccessToken ?? string.Empty;
        // }
        // finally
        // {
        //     semaphore.Release();
        // }
    }

    private async Task<string> RefreshTokenAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        AccessToken = null;
        AccessTokenExpiry = null;

        var propertyApiClient = scope.ServiceProvider.GetRequiredService<IPropertyApiClient>();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        if (string.IsNullOrEmpty(RefreshToken)) return string.Empty;

        var response = await propertyApiClient.RefreshTokenAsync(new RegenerateTokenDto
        {
            Token = RefreshToken,
            UserId = UserId
        });

        AccessToken = response.Token;
        AccessTokenExpiry = response.Expiry;

        var user = await localStorageService.GetItemAsync<AuthenticateResponseDto>("auth_state");
        user.Token = response.Token;
        user.TokenExpiry = response.Expiry;

        await localStorageService.SetItemAsync("auth_state", user);
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