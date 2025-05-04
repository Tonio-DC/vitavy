namespace Vitavy.Portal.Infrastructure.Keycloak.Models;

public class KeycloakRole
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}