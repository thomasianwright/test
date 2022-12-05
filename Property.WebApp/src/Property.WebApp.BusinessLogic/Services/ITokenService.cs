using System.Threading.Tasks;
using Property.WebApp.BusinessLogic.ApiClients;

namespace Caliqon.Property.WebApp.BusinessLogic.Services;

public interface ITokenService
{
    string UserId { get; }
    Task<string> GetTokenAsync();
    Task Logout();
    void SetAuthenticationState(AuthenticateResponseDto authenticateResponseDto);
}