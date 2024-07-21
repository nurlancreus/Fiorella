using AutoMapper;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Models;

namespace Fiorella.App.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagPostDto, Tag>().ReverseMap();
            CreateMap<TagUpdateDto, Tag>().ReverseMap();
            CreateMap<Tag, TagGetDto>();
        }
    }
}
