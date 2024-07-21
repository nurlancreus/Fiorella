using AutoMapper;
using Fiorella.App.Dtos.Blog;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Models;
using FluentValidation;

namespace Fiorella.App.Profiles
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogPostDto, Blog>().ReverseMap();
            CreateMap<BlogUpdateDto, Blog>().ReverseMap();
            //CreateMap<Blog, BlogGetDto>();

            // this will skip the image property if it's null or empty
            CreateMap<Blog, BlogGetDto>()
                .ForMember(dest => dest.Image, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Image)));
        }
    }
}
