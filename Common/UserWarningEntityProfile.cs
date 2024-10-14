using AutoMapper;
using Common.Models;
using Domain;

namespace Common;

public class UserWarningEntityProfile : Profile
{
    public UserWarningEntityProfile()
    {
        CreateMap<UserWarning, UserWarningModel>();
    }
}