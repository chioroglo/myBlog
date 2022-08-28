using AutoMapper;
using Domain;
using Entities;

namespace Mapping.MappingProfiles.cs
{
    public class CommentEntityProfile : Profile
    {
        public CommentEntityProfile()
        {
            CreateMap<CommentEntity, CommentModel>();
        }
    }
}
