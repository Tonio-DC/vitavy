namespace Vitavy.Portal.Infrastructure.Keycloak.Models;

public class KeycloakSetting
{
    public required string Base { get; set; }
    public required string Realm { get; set; }
    public required string ClientGuid { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}