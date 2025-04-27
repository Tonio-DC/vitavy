using AutoMapper;
using Vitavy.Portal.Application.Requests.Commands;
using Vitavy.Portal.Blazor.Models;

namespace Vitavy.Portal.Blazor.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Model -> Command
        CreateMap<PilotModel, LaunchPilotActionCommand>()
            .ForMember(cmd => cmd.Name, opt => opt.MapFrom(model => model.Name!))
            .ForMember(cmd => cmd.City, opt => opt.MapFrom(model => model.City!))
            .ForMember(cmd => cmd.Date, opt => opt.MapFrom(model => model.Date!))
            .ForMember(cmd => cmd.Name, opt => opt.MapFrom(model => model.Name!));
        
    }
}