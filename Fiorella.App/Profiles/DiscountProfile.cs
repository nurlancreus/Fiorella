using AutoMapper;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountPostDto, Discount>().ReverseMap();
            CreateMap<DiscountUpdateDto, Discount>().ReverseMap();
            CreateMap<Discount, DiscountGetDto>();
        }
    }
}
