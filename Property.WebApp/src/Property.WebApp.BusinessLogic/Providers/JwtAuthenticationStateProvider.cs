using System.Security.Claims;
using Blazored.LocalStorage;
using Caliqon.Property.WebApp.BusinessLogic.Helpers;
using Caliqon.Property.WebApp.BusinessLogic.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Property.WebApp.BusinessLogic.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ITokenService _tokenService;
    private readonly NavigationManager _navigationManager;
    public static string AuthStateStorageName = "auth_state";
    public JwtAuthenticationStateProvider(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        _localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
        _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
        _navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
    }

    private static AuthenticationState NotAuthenticatedState =
        new(new ClaimsPrincipal());

    private UserDto? _user;

    public string DisplayName => $"{_user.FirstName} {_user.LastName}";

    public bool IsLoggedIn => _user != null;


    public async Task Login(AuthenticateResponseDto loginDto)
    {
        await _localStorageService.SetItemAsync(AuthStateStorageName, loginDto);

        if (DateTime.Compare(loginDto.RefreshTokenExpiry.DateTime, DateTime.Now) <= 0)
            await Logout();
        
        _tokenService.SetAuthenticationState(loginDto);
        _user = loginDto.User;
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        Console.WriteLine($"Logged out {_user?.Email}");
        _user = null;
        
        await _localStorageService.RemoveItemAsync(AuthStateStorageName);
        
        await _tokenService.Logout();
        
        _navigationManager.NavigateTo("/authentication/logout");
        var state = await GetAuthenticationStateAsync();

        // Console.WriteLine(JsonConvert.SerializeObject(state));
        Console.WriteLine("Logged out!");

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return GetState();
    }

    private async Task<AuthenticationState> GetState()
    {
        var token = await _tokenService.GetTokenAsync();
        Console.WriteLine($"token from {nameof(GetState)} {token}");
        Console.WriteLine($"user is null : {_user == null}");
        var toReturn = _user != null
            ? new AuthenticationState(JwtSerialize.Deserialize(token))
            : NotAuthenticatedState;

        return toReturn;
    }
}