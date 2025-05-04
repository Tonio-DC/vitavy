using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Vitavy.Portal.Application.Infrastructure;
using Vitavy.Portal.Infrastructure.Keycloak.Handlers;
using Vitavy.Portal.Infrastructure.Keycloak.Mapping;
using Vitavy.Portal.Infrastructure.Keycloak.Models;
using Vitavy.Portal.Infrastructure.Keycloak.Services;
using Vitavy.Portal.Infrastructure.Keycloak.Services.Implementations;
using Vitavy.Portal.Infrastructure.Keycloak.UserRole;

namespace Vitavy.Portal.Infrastructure.Keycloak.Extensions;

public static class DependenciesRegistration
{
    public static IServiceCollection AddKeycloakInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        
        services.AddAutoMapper(typeof(KeycloakMappingProfile));
        services.AddTransient<AuthRetryHandler>();
        
        services.AddHttpClient<IUserRoleRepository, UserRoleRepository>(client =>
            {
                var baseUrl = configuration.GetSection("Keycloak:BaseUrl").Value ??
                              throw new ArgumentNullException("Keycloak:BaseUrl");
                var realm = configuration.GetSection("Keycloak:Realm").Value ??  throw new ArgumentNullException("Keycloak:Realm");
                client.BaseAddress = new Uri($"{baseUrl}/admin/realms/{realm}/");
            })
            .AddHttpMessageHandler<AuthRetryHandler>()
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        services.AddHttpClient<ITokenService, TokenService>(client =>
            {
                var baseUrl = configuration.GetSection("Keycloak:BaseUrl").Value ??
                              throw new ArgumentNullException("Keycloak:BaseUrl");
                var realm = configuration.GetSection("Keycloak:Realm").Value ??  throw new ArgumentNullException("Keycloak:Realm");
                client.BaseAddress = new Uri($"{baseUrl}/realms/{realm}/");
            })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        
        services.Configure<KeycloakSetting>(keycloakCredential =>
        {
            configuration.GetSection("Keycloak").Bind(keycloakCredential);
        });
        
        return services;
    }
}