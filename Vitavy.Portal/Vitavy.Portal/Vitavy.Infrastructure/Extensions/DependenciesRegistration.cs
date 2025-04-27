using Microsoft.Extensions.DependencyInjection;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Infrastructure.EventBus;

namespace Vitavy.Infrastructure.Extensions;

public static class DependenciesRegistration
{
    public static IServiceCollection AddEventHubInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IEventBusConsumer, EventBusConsumer>();
        services.AddTransient(typeof(IEventBusProducer<>), typeof(EventBusProducer<>));
        return services;
    }
}