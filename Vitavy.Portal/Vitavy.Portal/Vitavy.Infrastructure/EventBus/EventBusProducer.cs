using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Vitavy.Infrastructure.Contracts;
using Vitavy.Infrastructure.Exceptions;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Infrastructure.EventBus;

public class EventBusProducer<T>(IOptions<RabbitMqSetting> rabbitMqSetting, ILogger<EventBusProducer<T>> logger) : IEventBusProducer<T> where T : class
{
    //TODO: move initialization phase in constructor
    public async Task Publish(T message, string routingKey, string producerId, CancellationToken stoppingToken)
    {
        logger.LogInformation($"Publishing message to RabbitMQ  producer: {producerId}");
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSetting.Value.ServerCredentials?.Hostname ??
                       throw new RabbitMqMissingCredentialException("Hostname is missing."),
            Port = rabbitMqSetting.Value.ServerCredentials?.Port ??
                   throw new RabbitMqMissingCredentialException("Port is missing."),
            UserName = rabbitMqSetting.Value.ServerCredentials?.Username ??
                       throw new RabbitMqMissingCredentialException("Username is missing."),
            Password = rabbitMqSetting.Value.ServerCredentials?.Password ??
                       throw new RabbitMqMissingCredentialException("Password is missing.")
        };

        var producerCredential = rabbitMqSetting.Value.GetProducerCredential(producerId);

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        logger.LogInformation($"RabbitMQ connection created for  producer: {producerId}");
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        logger.LogInformation($"RabbitMQ channel created for  producer: {producerId}");
        
        await channel.ExchangeDeclareAsync(exchange: producerCredential.ExchangeName, type: producerCredential.Type.ToString(), cancellationToken: stoppingToken);
        logger.LogInformation($"RabbitMQ exchange declared for  producer: {producerId}");
        
        var serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        await channel.BasicPublishAsync(exchange: producerCredential.ExchangeName, routingKey: routingKey, body: body, cancellationToken: stoppingToken);
        logger.LogInformation($"RabbitMQ message published for  producer: {producerId}");
        
    }
}