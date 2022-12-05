using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using Property.WebApp.BusinessLogic.ApiClients;
using Property.WebApp.BusinessLogic.Providers;

namespace Property.WebApp.Pages.Authentication;

public partial class Login
{
    [Parameter] public string ReturnUrl { get; set; }
    public LoginDto LoginDto { get; set; }
    [Inject] private IPropertyApiClient ApiClient { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private JwtAuthenticationStateProvider JwtAuthenticationStateProvider { get; set; }

    protected override void OnInitialized()
    {
        LoginDto = new LoginDto();
        
        base.OnInitialized();
    }

    private async Task SubmitLogin()
    {
        try
        {
            var result = await ApiClient.LoginAsync(LoginDto);
            if (result is not null)
            {
                await JwtAuthenticationStateProvider.Login(result);
                
                NavigationManager.NavigateTo(ReturnUrl);
            } 
        }
        catch (ApiException e)
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(e.Response);
            Snackbar.Add(errorResponse?.Message ?? "An unknown error occured", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.GetType().FullName, Severity.Error);
            Snackbar.Add(e.Message, Severity.Error);
        }
    }
}