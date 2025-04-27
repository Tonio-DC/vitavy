using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vitavy.Infrastructure.Models;

namespace Vitavy.Portal.Infrastructure.Extensions;

public static class DependenciesRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSetting>( rabbitMqCredential =>
        {
            configuration.GetSection("RabbitMq").Bind(rabbitMqCredential);
        });
        return services;
    }
}