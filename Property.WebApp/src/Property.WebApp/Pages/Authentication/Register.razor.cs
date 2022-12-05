using System.Globalization;
using System.Text.RegularExpressions;
using ISO3166;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Property.WebApp.Pages.Authentication;

public partial class Register
{
    private CreateUserDto registerDto;
    private MudForm userDetailsForm;
    private MudForm addressForm;
    private bool success;
    private string[] userErrors = { };
    private string[] addressErrors = { };
    private IEnumerable<string> Countries = new List<string>();

    [Inject] private IPropertyApiClient _propertyApiClient { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }

    protected override void OnInitialized()
    {
        registerDto = new CreateUserDto();
        registerDto.Address = new CreateAddressDto();
        Countries = GetCountries();

        base.OnInitialized();
    }

    private IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }

        if (pw.Length < 8)
            yield return "Password must be at least of length 8";
        if (!Regex.IsMatch(pw, @"[A-Z]"))
            yield return "Password must contain at least one capital letter";
        if (!Regex.IsMatch(pw, @"[a-z]"))
            yield return "Password must contain at least one lowercase letter";
        if (!Regex.IsMatch(pw, @"[0-9]"))
            yield return "Password must contain at least one digit";
    }

    private string? PasswordMatch(string arg)
    {
        return registerDto.Password != arg ? "Passwords don't match" : null;
    }

    private IEnumerable<string> GetCountries()
    {
        return Country.List.Select(c => c.Name).ToList();
    }

    private async Task Submit()
    {
        await addressForm.Validate();
        await userDetailsForm.Validate();

        if (userDetailsForm.IsValid && addressForm.IsValid)
        {
            try
            {
                var result = await _propertyApiClient.RegisterAsync(registerDto);
                if (result is not null)
                {
                    _navigationManager.NavigateTo("/authentication/login");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}