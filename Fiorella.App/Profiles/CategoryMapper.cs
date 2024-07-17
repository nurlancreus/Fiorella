using AutoMapper;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryPostDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryGetDto>();
        }
    }
}
