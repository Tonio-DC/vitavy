namespace Vitavy.Infrastructure.Exceptions;

public class RabbitMqMissingCredentialException(string credentialId) : Exception
{
    public string CredentialId { get; init; } = credentialId;
}