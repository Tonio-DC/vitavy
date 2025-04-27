using Vitavy.Infrastructure.Exceptions;

namespace Vitavy.Infrastructure.Models;

public class RabbitMqSetting
{
    public RabbitMqCredential? ServerCredentials { get; set; }
    public required IEnumerable<EventBusProducerCredential> EventBusProducersCredentials { get; set; }
    public required IEnumerable<EventBusConsumerCredential> EventBusConsumersCredentials { get; set; }

    public EventBusProducerCredential GetProducerCredential(string producerId)
    {
        var producerCredential = EventBusProducersCredentials.FirstOrDefault(c => c.Id == producerId) ?? throw new RabbitMqMissingCredentialException(producerId);
        return producerCredential;
    }

    public EventBusConsumerCredential GetConsumerCredential(string consumerId)
    {
        var consumerCredential = EventBusConsumersCredentials.FirstOrDefault(c => c.Id == consumerId) ?? throw new RabbitMqMissingCredentialException(consumerId);
        return consumerCredential;
    }
}

public class EventBusProducerCredential
{
    public string Id { get; set; }
    public string ExchangeName { get; set; }
    public ExchangeType Type { get; set; }
}

//TODO make these classes records and add validation rules (ie exceptions?) regarding exchange types
public class EventBusConsumerCredential
{
    public string Id { get; set; }
    public string ExchangeName { get; set; }
    public ExchangeType Type { get; set; }
    public string QueueName { get; set; }
    public IEnumerable<string> BindingKeys { get; set; }
}

public enum ExchangeType
{
    Topic,
    Direct,
    Fanout,
    Headers,
    Default
}

public class RabbitMqCredential
{
    public string? Hostname { get; set; }
    public int? Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}

