using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vitavy.Pilot.Application.Features.PilotAction;
using Vitavy.Pilot.Application.Mapping;

namespace Vitavy.Pilot.Application.Extensions;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationMappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(PilotActionCommand).Assembly));
        return services;
    }
}