using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Caliqon.Property.WebApp.BusinessLogic.MessageHandlers;
using Caliqon.Property.WebApp.BusinessLogic.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Property.WebApp;
using Property.WebApp.BusinessLogic.ApiClients;
using Property.WebApp.BusinessLogic.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IPropertyApiClient, PropertyApiClient>(((sp, client) =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue(
            "ApiUrl", "https://localhost:7049/")!);
    }))
    .AddHttpMessageHandler<JwtMessageHandler>();

builder.Services.AddScoped<JwtMessageHandler>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddSingleton<JwtAuthenticationStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireModeratorRole", policy => policy.RequireRole("Moderator"));
    options.AddPolicy("RequireCompanyUser", policy => policy.RequireRole("CompanyUser"));
    options.AddPolicy("RequireTenantRole", policy => policy.RequireRole("TenantUser"));
});


await builder.Build().RunAsync();