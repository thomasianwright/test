using System.Security.Claims;
using Blazored.LocalStorage;
using Caliqon.Property.WebApp.BusinessLogic.Helpers;
using Caliqon.Property.WebApp.BusinessLogic.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Property.WebApp.BusinessLogic.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ITokenService _tokenService;
    private readonly NavigationManager _navigationManager;

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


    public async Task Login(AuthenticateResponseDto? loginDto = null)
    {
        var authenticationState = await _localStorageService.GetItemAsync<AuthenticateResponseDto>("auth_state");

        if (loginDto is not null)
        {
            await _localStorageService.SetItemAsync("auth_state", loginDto);
            authenticationState = loginDto;
        }

        if (authenticationState is not null)
        {
            _tokenService.SetAuthenticationState(authenticationState);
            _user = authenticationState.User;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        _navigationManager.NavigateTo("/authentication/login");
    }

    public Task Logout()
    {
        _user = null;
        NotifyAuthenticationStateChanged(GetState());

        return Task.CompletedTask;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return GetState();
    }

    private async Task<AuthenticationState> GetState()
    {
        return _user != null
            ? new AuthenticationState(JwtSerialize.Deserialize(await _tokenService.GetTokenAsync()))
            : NotAuthenticatedState;
    }
}