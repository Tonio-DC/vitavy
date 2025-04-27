using AutoMapper;
using Vitavy.Pilot.Application.Features.PilotAction;
using Vitavy.Pilot.Application.Models;

namespace Vitavy.Pilot.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<LauchPilotActionMessage, PilotActionCommand>();
    }
}