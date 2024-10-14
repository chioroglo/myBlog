﻿using AutoMapper;
using Common.Dto.Auth;
using Common.Dto.User;
using Common.Models;
using Domain;

namespace Common.MappingProfiles
{
    public class UserEntityProfile : Profile
    {
        public UserEntityProfile()
        {
            CreateMap<User, AuthenticateResponse>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Token, opt => opt.Ignore());

            CreateMap<RegistrationDto, User>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.FirstName,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FirstName) ? null : src.FirstName))
                .ForMember(dst => dst.LastName,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LastName) ? null : src.LastName))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.RegistrationDate, opt => opt.NullSubstitute(DateTime.UtcNow));

            CreateMap<User, UserModel>()
                .ForMember(
                    dst => dst.FullName,
                    opt => opt.MapFrom(
                        src => src.FirstName == null && src.LastName == null
                            ? string.Empty
                            : $"{src.FirstName} {src.LastName}"
                    ))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate.ToString("u")))
                .ForMember(dst => dst.LastActivity, opt => opt.MapFrom(src => src.LastActivity.ToString("u")))
                .ForMember(dst => dst.ActiveWarnings, opt => opt.MapFrom(src => src.Warnings));


            CreateMap<UserInfoDto, User>().ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.FirstName,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FirstName) ? null : src.FirstName))
                .ForMember(dst => dst.LastName,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LastName) ? null : src.LastName))
                .ForMember(dst => dst.RegistrationDate, opt => opt.NullSubstitute(DateTime.UtcNow));
        }
    }
}