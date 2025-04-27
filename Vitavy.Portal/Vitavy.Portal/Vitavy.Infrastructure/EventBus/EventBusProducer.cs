using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Vitavy.Infrastructure.Contracts;
using Vitavy.Infrastructure.Exceptions;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Infrastructure.EventBus;

public class EventBusProducer<T> : IEventBusProducer<T> where T : class
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IOptions<RabbitMqSetting> _rabbitMqSetting;
    private readonly ILogger<EventBusProducer<T>> _logger;

    public EventBusProducer(IOptions<RabbitMqSetting> rabbitMqSetting, ILogger<EventBusProducer<T>> logger)
    {
        _logger = logger;
        _rabbitMqSetting = rabbitMqSetting;
        _connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.Value.ServerCredentials?.Hostname ??
                       throw new RabbitMqMissingCredentialException("Hostname is missing."),
            Port = _rabbitMqSetting.Value.ServerCredentials?.Port ??
                   throw new RabbitMqMissingCredentialException("Port is missing."),
            UserName = _rabbitMqSetting.Value.ServerCredentials?.Username ??
                       throw new RabbitMqMissingCredentialException("Username is missing."),
            Password = _rabbitMqSetting.Value.ServerCredentials?.Password ??
                       throw new RabbitMqMissingCredentialException("Password is missing.")
        };
    }
    
    public async Task Publish(T message, string routingKey, string producerId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Publishing message to RabbitMQ  producer: {producerId}");
        var producerCredential = _rabbitMqSetting.Value.GetProducerCredential(producerId);

        await using var connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        _logger.LogInformation($"RabbitMQ connection created for  producer: {producerId}");
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        _logger.LogInformation($"RabbitMQ channel created for  producer: {producerId}");
        
        await channel.ExchangeDeclareAsync(exchange: producerCredential.ExchangeName, type: producerCredential.Type.ToString(), cancellationToken: stoppingToken);
        _logger.LogInformation($"RabbitMQ exchange declared for  producer: {producerId}");
        
        var serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        await channel.BasicPublishAsync(exchange: producerCredential.ExchangeName, routingKey: routingKey, body: body, cancellationToken: stoppingToken);
        _logger.LogInformation($"RabbitMQ message published for  producer: {producerId}");
        
    }
}