namespace Vitavy.Infrastructure.Contracts;

public interface IEventBusProducer<in T> where T : class
{
    Task Publish(T message, string routingKey, string producerId, CancellationToken stoppingToken);
}