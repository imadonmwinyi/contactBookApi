using AutoMapper;
using ContactBookAPI.DTOs;
using ContactBookAPI.Lib.Model;

namespace ContactBookAPI.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserDto>()
                      .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>$"{src.FirstName} {src.LastName}"))
                      .ForMember(dest=>dest.Address, opt=>opt
                                .MapFrom(src=>$"{src.City}, {src.State} State, {src.Country}"));
            CreateMap<AppUser, AfterPhotoUpdateDto>()
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
