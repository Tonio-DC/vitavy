using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Vitavy.Portal.Infrastructure.Keycloak.Models;

namespace Vitavy.Portal.Infrastructure.Keycloak.Services.Implementations;

public class TokenService(HttpClient httpClient, IOptions<KeycloakSetting> keycloakSetting) : ITokenService
{
    public async Task<string> RefreshTokenAsync()
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", keycloakSetting.Value.ClientId },
            { "client_secret", keycloakSetting.Value.ClientSecret },
            { "grant_type", "client_credentials" }
        };
        var content = new FormUrlEncodedContent(values);
        var response = await httpClient.PostAsync("protocol/openid-connect/token", content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenResponse?.AccessToken ?? throw new Exception("Échec de la régénération du token");
    }
}