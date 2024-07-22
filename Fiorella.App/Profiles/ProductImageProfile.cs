using AutoMapper;
using Fiorella.App.Dtos.ProductImage;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImagePostDto, ProductImage>().ReverseMap();
            CreateMap<ProductImageUpdateDto, ProductImage>().ReverseMap();

            // this will skip the image property if it's null or empty
            CreateMap<ProductImage, ProductImageGetDto>()
                .ForMember(dest => dest.Url, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Url)));
        }
    }
}
