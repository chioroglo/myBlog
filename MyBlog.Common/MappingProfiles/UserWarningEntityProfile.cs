using AutoMapper;
using MyBlog.Common.Models;
using MyBlog.Domain;

namespace MyBlog.Common.MappingProfiles;

public class UserWarningEntityProfile : Profile
{
    public UserWarningEntityProfile()
    {
        CreateMap<UserWarning, UserWarningModel>();
    }
}