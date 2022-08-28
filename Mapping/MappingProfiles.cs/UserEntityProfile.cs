using AutoMapper;
using Domain;
using Domain.Dto.Account;
using Entities;

namespace Mapping.MappingProfiles.cs
{
    public class UserEntityProfile : Profile
    {
        public UserEntityProfile()
        {
            CreateMap<UserEntity, AuthenticateResponse>().
                ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dst => dst.LastActivity, opt => opt.MapFrom(src => src.LastActivity))
                .ForMember(dst => dst.Token, opt => opt.Ignore());

            CreateMap<RegistrationDto, UserEntity>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.RegistrationDate, opt => opt.NullSubstitute(DateTime.UtcNow));

            CreateMap<UserEntity, UserModel>()
                .ForMember(
                dst => dst.FullName,
                opt => opt.MapFrom(
                    src => $"{src.FirstName} {src.LastName}"));
        }
    }
}