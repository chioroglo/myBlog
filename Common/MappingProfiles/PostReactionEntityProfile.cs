﻿using AutoMapper;
using Common.Dto.PostReaction;
using Common.Models;
using Domain;

namespace Common.MappingProfiles
{
    public class PostReactionEntityProfile : Profile
    {
        public PostReactionEntityProfile()
        {
            CreateMap<PostReaction, PostReactionDto>()
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

            CreateMap<PostReactionDto, PostReaction>()
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

            CreateMap<PostReaction, PostReactionModel>()
                .ForMember(e => e.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

            CreateMap<PostReactionDto, PostReactionModel>()
                .ForMember(e => e.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(e => e.ReactionType, opt => opt.MapFrom(src => src.ReactionType));
        }
    }
}