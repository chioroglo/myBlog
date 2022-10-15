using AutoMapper;
using Common.Dto.Post;
using Common.Models;
using Domain;

namespace Common.MappingProfiles
{
    public class PostEntityProfile : Profile
    {
        public PostEntityProfile()
        {
            CreateMap<Post, PostModel>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dst => dst.AuthorUsername, opt => opt.MapFrom(src => src.User.Username));

            CreateMap<PostModel, Post>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<PostDto, Post>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));
        }
    }
}
