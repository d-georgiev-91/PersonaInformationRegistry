using AutoMapper;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Domain.Entities;

namespace PersonalInformationRegistry.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Credentials!.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Credentials!.Password));

        CreateMap<CreatePersonCommand, Person>()
            .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src => new Credentials(src.Email, src.Password)));

        CreateMap<UpdatePersonCommand, Person>()
            .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src => new Credentials(src.Email, src.Password)));
    }
}