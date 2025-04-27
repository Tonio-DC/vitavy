using Microsoft.Extensions.DependencyInjection;
using Vitavy.Portal.Application.Validators;
using Vitavy.Portal.Application.Validators.Implementations;

namespace Vitavy.Portal.Application.Extensions;

public static class ServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IPilotCommandValidator, PilotCommandValidator>();
        return services;
    }
}