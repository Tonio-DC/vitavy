using System.Net;
using System.Net.Http.Headers;
using Vitavy.Portal.Infrastructure.Keycloak.Services;

namespace Vitavy.Portal.Infrastructure.Keycloak.Handlers;

public class AuthRetryHandler(ITokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var newToken = await tokenService.RefreshTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

            // Retenter la requête avec le nouveau token
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}