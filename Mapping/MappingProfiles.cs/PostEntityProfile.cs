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
            CreateMap<PostEntity, PostModel>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<PostModel,PostEntity>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<PostDto, PostModel>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));
        }
    }
}
