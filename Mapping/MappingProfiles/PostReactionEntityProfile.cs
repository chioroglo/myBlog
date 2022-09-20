using AutoMapper;
using Domain;
using Domain.Dto.PostReaction;

namespace Mapping.MappingProfiles
{
    public class PostReactionEntityProfile : Profile
    {
        public PostReactionEntityProfile()
        {
            CreateMap<PostReaction, PostReactionDto>()
                .ForMember(e => e.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

            CreateMap<PostReactionDto, PostReaction>()
                .ForMember(e => e.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

        }
    }
}
