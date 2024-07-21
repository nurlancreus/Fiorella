using AutoMapper;
using Fiorella.App.Dtos.Employee;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class EmployeeProfile : Profile
    {

        public EmployeeProfile()
        {
            CreateMap<EmployeePostDto, Employee>().ReverseMap();
            CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();
            //CreateMap<Employee, EmployeeGetDto>();

            // this will skip the image property if it's null or empty
            CreateMap<Employee, EmployeeGetDto>()
                    .ForMember(dest => dest.Image, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Image)));

        }
    }
}
