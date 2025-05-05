namespace Vitavy.Portal.Infrastructure.Keycloak.Services;

public interface ITokenService
{
    public Task<string> RefreshTokenAsync();
}