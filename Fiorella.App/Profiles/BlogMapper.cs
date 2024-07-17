using AutoMapper;
using Fiorella.App.Dtos.Blog;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Models;
using FluentValidation;

namespace Fiorella.App.Profiles
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<BlogPostDto, Blog>();
            CreateMap<BlogUpdateDto, Blog>();
            CreateMap<Blog, BlogGetDto>();

        }
    }
}
