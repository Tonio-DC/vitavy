using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Infrastructure.Exceptions;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Infrastructure.EventBus;

public class EventBusRpcProducer<TMessage, TResponse> : IEventBusRpcProducer<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _callbackMapper;

    private IConnection? _connection;
    private IChannel? _channel;
    private string? _replyQueueName;
    private readonly IOptions<RabbitMqSetting> _rabbitMqSetting;
    private readonly ILogger<EventBusRpcProducer<TMessage, TResponse>> _logger;

    public EventBusRpcProducer(IOptions<RabbitMqSetting> rabbitMqSetting, ILogger<EventBusRpcProducer<TMessage, TResponse>> logger)
    {
        _logger = logger;
        _rabbitMqSetting = rabbitMqSetting;
        _callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<TResponse>>();
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

    public async Task StartRpc(string producerId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Initializing Remote procedure call (RPC)");

        _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        _logger.LogInformation($"RabbitMQ connection created for  producer: {producerId}");
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        _logger.LogInformation($"RabbitMQ channel created for  producer: {producerId}");
        
        // declare a RPC response queue from config or auto-generated if from config is empty
        QueueDeclareOk queueDeclareResult =
            await _channel.QueueDeclareAsync(
                queue: _rabbitMqSetting.Value.EventBusProducersCredentials.Rpc.RpcQueueName ?? string.Empty,
                durable: true,
                cancellationToken: stoppingToken);
        _replyQueueName = queueDeclareResult.QueueName;
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (_, ea) =>
        {
            var correlationId = ea.BasicProperties.CorrelationId;

            if (string.IsNullOrEmpty(correlationId))
                return Task.CompletedTask;
            
            if (_callbackMapper.TryRemove(correlationId, out var tcs))
            {
                var body = ea.Body.ToArray();
                var serializedResponse = Encoding.UTF8.GetString(body);
                var response = JsonSerializer.Deserialize<TResponse>(serializedResponse);
                tcs.TrySetResult(response!);
            }

            return Task.CompletedTask;
        };
        await _channel.BasicConsumeAsync(_replyQueueName, false, consumer, cancellationToken: stoppingToken);
    }

    public async Task<TResponse> PublishRpc(TMessage message, string routingKey, string producerId, CancellationToken stoppingToken)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException();
        }
        var correlationId = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = _replyQueueName
        };
        var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, tcs);
        
        var producerCredential = _rabbitMqSetting.Value.GetProducerCredential(producerId);

        var serializedMessage = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);
        await _channel.BasicPublishAsync(exchange: producerCredential.ExchangeName, routingKey: routingKey,
            mandatory: true, basicProperties: props, body: messageBytes, cancellationToken: stoppingToken);

        await using CancellationTokenRegistration ctr =
            stoppingToken.Register(() =>
            {
                _callbackMapper.TryRemove(correlationId, out _);
                tcs.SetCanceled(stoppingToken);
            });

        return await tcs.Task;
    }
}