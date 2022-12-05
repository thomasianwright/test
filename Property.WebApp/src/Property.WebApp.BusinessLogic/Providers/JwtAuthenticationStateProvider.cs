using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Caliqon.Property.WebApp.BusinessLogic.Helpers;
using Caliqon.Property.WebApp.BusinessLogic.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Property.WebApp.Providers;

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
        new (new ClaimsPrincipal());

    private UserDto? _user;

    /// <summary>
    /// The display name of the user.
    /// </summary>
    public string DisplayName => $"{_user.FirstName} {_user.LastName}";

    /// <summary>
    /// <see langword="true"/> if there is a user logged in, otherwise false.
    /// </summary>
    public bool IsLoggedIn => _user != null;

    /// <summary>
    /// Login the user with a given JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    public async Task Login(AuthenticateResponseDto? loginDto)
    {
        var authenticationState = await _localStorageService.GetItemAsync<AuthenticateResponseDto>("auth_state");

        if (authenticationState is null && loginDto is not null)
        {
            await _localStorageService.SetItemAsync("auth_state", loginDto);
        }
        
        if (authenticationState is not null || loginDto is not null)
        {
            _tokenService.SetAuthenticationState(authenticationState ?? loginDto);
            _user = authenticationState?.User;
        }
    }

    /// <summary>
    /// Logout the current user.
    /// </summary>
    public Task Logout()
    {
        _user = null;
        NotifyAuthenticationStateChanged(GetState());
        
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return GetState();
    }

    /// <summary>
    /// Constructs an authentication state.
    /// </summary>
    /// <returns>The created state.</returns>
    private async Task<AuthenticationState> GetState()
    {
        return _user != null
            ? new AuthenticationState(JwtSerialize.Deserialize(await _tokenService.GetTokenAsync()))
            : NotAuthenticatedState;
    }
}