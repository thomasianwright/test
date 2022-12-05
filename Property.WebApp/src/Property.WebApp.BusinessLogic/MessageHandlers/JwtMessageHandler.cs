using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Caliqon.Property.WebApp.BusinessLogic.Services;

namespace Caliqon.Property.WebApp.BusinessLogic.MessageHandlers;

public class JwtMessageHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public JwtMessageHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tokenTask = _tokenService.GetTokenAsync();

        var token = tokenTask.Result;
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenService.GetTokenAsync());
        
        return await base.SendAsync(request, cancellationToken);
    }
}