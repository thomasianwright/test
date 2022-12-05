using Blazored.LocalStorage;
using Caliqon.Property.WebApp.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Property.WebApp.BusinessLogic.Tests.Services;

public class TokenServiceTests
{
    public TokenServiceTests()
    {
        
    }
    
    [Fact]
    public void GenerateTokenTest()
    {
        var serviceProvider = CreateServiceProvider((typeof(ILocalStorageService), typeof(IPropertyApiClient)));
        var tokenService = new TokenService(serviceProvider.Object);
        
        var token = tokenService.GetTokenAsync();
        Assert.NotNull(token);
    }

    private Mock<IServiceProvider> CreateServiceProvider(params (Type @interface, Object service)[] services)
    {
        var scopedServiceProvider = new Mock<IServiceProvider>();

        foreach (var (@interfcae, service) in services)
        {
            scopedServiceProvider
                .Setup(s => s.GetService(@interfcae))
                .Returns(service);
        }

        var scope = new Mock<IServiceScope>();
        scope
            .SetupGet(s => s.ServiceProvider)
            .Returns(scopedServiceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(scope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactory.Object);

        return serviceProvider;
    }
}