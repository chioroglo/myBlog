using AutoMapper;
using Domain;
using Domain.Models;

namespace Mapping.MappingProfiles.cs
{
    public class CommentEntityProfile : Profile
    {
        public CommentEntityProfile()
        {
            CreateMap<Comment, CommentModel>();
        }
    }
}
