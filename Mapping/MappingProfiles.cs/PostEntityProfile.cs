using AutoMapper;
using Domain;
using Domain.Dto.Post;
using Entities;

namespace Mapping.MappingProfiles.cs
{
    public  class PostEntityProfile : Profile
    {
        public PostEntityProfile()
        {
            CreateMap<PostEntity, PostModel>();

            CreateMap<PostDto, PostEntity>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.AuthorId));
        }
    }
}
