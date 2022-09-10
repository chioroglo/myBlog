using AutoMapper;
using Domain;
using Domain.Dto.Comment;
using Domain.Models;

namespace Mapping.MappingProfiles.cs
{
    public class CommentEntityProfile : Profile
    {
        public CommentEntityProfile()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(e => e.Content,opt => opt.MapFrom(e => e.Content))
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(e => e.RegistrationDate, opt => opt.MapFrom(e => e.RegistrationDate))
                .ForMember(e => e.AuthorId, opt => opt.MapFrom(e => e.UserId))
                .ForMember(e => e.PostId, opt => opt.MapFrom(e => e.PostId));

            CreateMap<CommentDto, Comment>()
                .ForMember(e => e.Content, opt => opt.MapFrom(e => e.Content))
                .ForMember(e => e.PostId, opt => opt.MapFrom(e => e.PostId))
                .ForMember(e => e.UserId, opt => opt.MapFrom(e => e.AuthorId));
        }
    }
}
