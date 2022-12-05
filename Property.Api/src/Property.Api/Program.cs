using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HashidsNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Property.Api.BusinessLogic.MappingProfiles;
using Property.Api.BusinessLogic.Options;
using Property.Api.BusinessLogic.Services;
using Property.Api.Contracts.Repositories;
using Property.Api.Contracts.Services;
using Property.Api.Infrastructure.Data;
using Property.Api.Infrastructure.Repositories;
    
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors((options) =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddControllers(options =>
    {
        options.OutputFormatters.RemoveType<StringOutputFormatter>();
        options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    })
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Property API", Version = "v1" });
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
});

builder.Services.AddDbContext<ApiContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("ApiConnection"),
        optionsBuilder => { optionsBuilder.MigrationsAssembly(typeof(ApiContext).Assembly.FullName); });
});

// Configure IOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRentalAgreementRepository, RentalAgreementRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();

// Services
builder.Services.AddAutoMapper(typeof(UserMapping).Assembly);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IHashids>(_ => new Hashids(builder.Configuration["Hashids:Salt"], 8));
builder.Services.AddTransient<ITokenService, TokenService>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var jwtOptions = new JwtOptions();
    builder.Configuration.Bind("JwtOptions", jwtOptions);
    
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();