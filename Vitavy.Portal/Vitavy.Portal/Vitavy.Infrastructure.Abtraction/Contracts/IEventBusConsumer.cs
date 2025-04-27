using FluentResults;

namespace Vitavy.Infrastructure.Abtraction.Contracts;

public interface IEventBusConsumer
{
    Task Subscribe<TMessage, TResponse>(Func<TMessage, CancellationToken, Task<Result<TResponse>>> processMessageAsync, string consumerId, CancellationToken stoppingToken);
}