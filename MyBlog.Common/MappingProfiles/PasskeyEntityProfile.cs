using AutoMapper;
using MyBlog.Common.Models.Passkey;
using MyBlog.Domain;

namespace MyBlog.Common.MappingProfiles;

public class PasskeyEntityProfile : Profile
{
    public PasskeyEntityProfile()
    {
        CreateMap<Passkey, PasskeyInfoModel>()
            .ForMember(e => e.Name, opt => opt.MapFrom(e => $"Passkey №{e.Id}"));

        CreateMap<IEnumerable<Passkey>, PasskeyListModel>()
            .ForMember(e => e.Passkeys, opt => opt.MapFrom(src => src.ToList()));
    }
}