using AutoMapper;
using Domain;
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
                .ForMember(e => e.AuthorUsername, opt => opt.MapFrom(e => e.User.Username));
        }
    }
}
