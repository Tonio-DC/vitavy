using System.Text.Json.Serialization;

namespace Vitavy.Portal.Infrastructure.Keycloak.Models;

public class KeycloakUser
{
    [JsonPropertyName("id")]
    public required string GuidId { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}