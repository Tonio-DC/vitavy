using AutoMapper;
using Vitavy.Portal.Application.Models;
using Vitavy.Portal.Infrastructure.Keycloak.Models;

namespace Vitavy.Portal.Infrastructure.Keycloak.Mapping;

public class KeycloakMappingProfile : Profile
{
    public KeycloakMappingProfile()
    {
        CreateMap<KeycloakUser, User>()
            .ForMember(user => user.Id, opt => opt.MapFrom(keycloakUser => Guid.Parse(keycloakUser.GuidId)))
            .ForMember(user => user.Firstname, opt => opt.MapFrom(keycloakUser => keycloakUser.FirstName))
            .ForMember(user => user.Lastname, opt => opt.MapFrom(keycloakUser => keycloakUser.LastName));

        CreateMap<KeycloakRole, Role>()
            .ForMember(role => role.Id, opt => opt.MapFrom(keycloakRole => Guid.Parse(keycloakRole.Id)));

        CreateMap<Role, KeycloakRole>()
            .ForMember(keycloakRole => keycloakRole.Id, opt => opt.MapFrom(role => role.Id.ToString()));

    }
}