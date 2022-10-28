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
                .ForMember(dst => dst.AuthorUsername, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dst => dst.Topic, opt => opt.MapFrom(src => src.Topic == null ? "" : src.Topic.Name))
                .ForMember(dst => dst.AmountOfComments, opt => opt.MapFrom(src => src.Comments.Count()));

            //CreateMap<PostModel, Post>()
            //    .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.AuthorId))
            //    .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<PostDto, Post>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dst => dst.Topic, opt => opt.MapFrom(e => new Topic()))
                .ForPath(dst => dst.Topic.Name, opt => opt.MapFrom(e => e.Topic));
        }
    }
}
