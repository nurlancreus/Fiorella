using AutoMapper;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryPostDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
            CreateMap<Category, CategoryGetDto>();
        }
    }
}
