﻿using AutoMapper;
using Common.Models.Passkey;
using Domain;

namespace Common.MappingProfiles;

public class PasskeyEntityProfile : Profile
{
    public PasskeyEntityProfile()
    {
        CreateMap<Passkey, PasskeyInfoModel>()
            .ForMember(e => e.Name, opt => opt.MapFrom(e => $"Passkey №{e.Id}"));
    }
}