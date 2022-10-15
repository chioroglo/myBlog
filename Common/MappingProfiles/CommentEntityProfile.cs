using AutoMapper;
using Common.Dto.Comment;
using Common.Models;
using Domain;

namespace Common.MappingProfiles;

public class CommentEntityProfile : Profile
{
    public CommentEntityProfile()
    {
        CreateMap<Comment, CommentModel>()
            .ForMember(e => e.Content, opt => opt.MapFrom(e => e.Content))
            .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Id))
            .ForMember(e => e.RegistrationDate, opt => opt.MapFrom(e => e.RegistrationDate))
            .ForMember(e => e.AuthorId, opt => opt.MapFrom(e => e.UserId))
            .ForMember(e => e.PostId, opt => opt.MapFrom(e => e.PostId))
            .ForMember(e => e.AuthorUsername, opt => opt.MapFrom(e => e.User.Username))
            .ForMember(e => e.PostTitle, opt => opt.MapFrom(e => e.Post.Title));

        CreateMap<CommentDto, Comment>()
            .ForMember(e => e.Content, opt => opt.MapFrom(e => e.Content))
            .ForMember(e => e.PostId, opt => opt.MapFrom(e => e.PostId))
            .ForMember(e => e.UserId, opt => opt.MapFrom(e => e.AuthorId));
    }
}