using AutoMapper;
using Fiorella.App.Dtos.Position;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            //CreateMap<PositionDto, Position>().ReverseMap();
            CreateMap<Position, PositionDto>()
                            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
                            .ReverseMap();
        }
    }
}
