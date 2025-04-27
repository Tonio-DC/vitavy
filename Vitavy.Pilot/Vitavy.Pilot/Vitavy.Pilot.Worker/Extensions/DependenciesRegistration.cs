using Vitavy.Pilot.Worker.BackgroundServices;

namespace Vitavy.Pilot.Worker.Extensions;

public static class DependenciesRegistration
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddHostedService<PilotBackgroundService>();
        return services;
    }
}