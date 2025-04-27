namespace Vitavy.Infrastructure.Abtraction.Contracts;

public interface IEventBusProducer<in TMessage> where TMessage : class
{
    Task Publish(TMessage message, string routingKey, string producerId, CancellationToken stoppingToken);
}