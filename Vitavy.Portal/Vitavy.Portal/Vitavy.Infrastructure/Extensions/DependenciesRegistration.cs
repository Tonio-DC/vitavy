using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Infrastructure.EventBus;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Infrastructure.Extensions;

public static class DependenciesRegistration
{
    public static IServiceCollection AddEventHubInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBusConsumer, EventBusConsumer>();
        services.AddSingleton(typeof(IEventBusRpcProducer<,>), typeof(EventBusRpcProducer<,>));
        services.AddSingleton(typeof(IEventBusProducer<>), typeof(EventBusProducer<>));
        services.Configure<RabbitMqSetting>( rabbitMqCredential =>
        {
            configuration.GetSection("RabbitMq").Bind(rabbitMqCredential);
        });
        return services;
    }
}