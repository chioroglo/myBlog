using AutoMapper;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Models;

namespace MyBlog.Common.MappingProfiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<AuthorizationResponse, AuthorizationResponseModel>();
    }
}