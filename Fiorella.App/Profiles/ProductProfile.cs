using AutoMapper;
using Fiorella.App.Dtos.Product;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductPostDto, Product>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<Product, ProductGetDto>();
        }
    }
}
