using Microsoft.Extensions.DependencyInjection;
using Vitavy.Pilot.Application.Mapping;

namespace Vitavy.Pilot.Application.Extensions;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationMappingProfile));
        return services;
    }
}