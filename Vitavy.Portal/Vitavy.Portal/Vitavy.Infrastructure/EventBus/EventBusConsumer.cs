using System.Text;
using System.Text.Json;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Infrastructure.Exceptions;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Infrastructure.EventBus;

public class EventBusConsumer(IOptions<RabbitMqSetting> rabbitMqSetting, ILogger<EventBusConsumer> logger) : IEventBusConsumer
{
    
    public async Task Subscribe<TMessage, TResponse>(Func<TMessage, CancellationToken, Task<Result<TResponse>>> processMessageAsync, string consumerId, CancellationToken stoppingToken)
    {
        logger.LogInformation($"Subscribing to RabbitMQ consumer: {consumerId}");
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
        
        var consumerCredential = rabbitMqSetting.Value.GetConsumerCredential(consumerId);

        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(exchange: consumerCredential.ExchangeName, type: consumerCredential.Type.ToString().ToLowerInvariant(), cancellationToken: stoppingToken);

        // declare a server-named queue
        var queueDeclareResult = await channel.QueueDeclareAsync(queue: consumerCredential.QueueName, durable: true, cancellationToken: stoppingToken);
        //si le queueName est empty, il est auto-généré
        var queueName = queueDeclareResult.QueueName;

        foreach (var bindingKey in consumerCredential.BindingKeys)
        {
            await channel.QueueBindAsync(queue: queueName, exchange: consumerCredential.ExchangeName, routingKey: bindingKey, cancellationToken: stoppingToken);
        }
        
        await StartConsuming(channel, queueName, processMessageAsync, stoppingToken);
    }

    private async Task StartConsuming<TMessage, TResponse>(IChannel channel, string queueName,
        Func<TMessage, CancellationToken, Task<Result<TResponse>>> processMessageAsync, CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var parsedPayload = Encoding.UTF8.GetString(body);
            var payload = JsonSerializer.Deserialize<TMessage>(parsedPayload);

            Result<TResponse>? processedSuccessfully = null;

            if (payload is not null)
            {
                try
                {
                    processedSuccessfully = await processMessageAsync(payload, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        $"{nameof(Exception)} occurred while processing message from queue {queueName}: {ex}");
                }
            }
            else
            {
                logger.LogError($"Error occurred while deserializing message from queue {queueName}");
            }

            #region RPC
            
            var props = ea.BasicProperties;
            var originalConsumer = (AsyncEventingBasicConsumer)sender;
            if (!string.IsNullOrEmpty(props.CorrelationId))
            {
                var replyProps = new BasicProperties
                {
                    CorrelationId = props.CorrelationId
                };
                var response = processedSuccessfully!.ValueOrDefault;
                var serializedResponse = JsonSerializer.Serialize(response);
                var responseBytes = Encoding.UTF8.GetBytes(serializedResponse);
                await originalConsumer.Channel.BasicPublishAsync(exchange: string.Empty, routingKey: props.ReplyTo!,
                    mandatory: true, basicProperties: replyProps, body: responseBytes, cancellationToken: stoppingToken);
            }
            
            #endregion

            if (processedSuccessfully is { IsSuccess: true })
            {
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false,
                    cancellationToken: stoppingToken);
            }
            else
            {
                await channel.BasicRejectAsync(deliveryTag: ea.DeliveryTag, requeue: true,
                    cancellationToken: stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer,
            cancellationToken: stoppingToken);
    }
}