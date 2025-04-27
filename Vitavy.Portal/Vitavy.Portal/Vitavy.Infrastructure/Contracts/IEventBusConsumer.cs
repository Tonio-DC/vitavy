using FluentResults;

namespace Vitavy.Infrastructure.Contracts;

public interface IEventBusConsumer
{
    Task Subscribe<T>(Func<T, CancellationToken, Task<Result>> processMessageAsync, string consumerId, CancellationToken stoppingToken);
}