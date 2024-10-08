using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Application.Models.ViewModels;
using Credit.Domain.Entities;

namespace Credit.Application.Mapper
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<PersonEntity, PersonViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                .ForMember(t => t.FirstName, o => o.MapFrom(t => t.FirstName))
                .ForMember(t => t.LastName, o => o.MapFrom(t => t.LastName))
                .ForMember(t => t.PersonalNumber, o => o.MapFrom(t => t.PersonalNumber))
                .ForMember(t => t.DateOfBirth, o => o.MapFrom(t => t.DateOfBirth))
                .ForMember(t => t.Email, o => o.MapFrom(t => t.Email));

            CreateMap<PersonDtoModel, PersonEntity>()
                .ForMember(t => t.FirstName, o => o.MapFrom(t => t.FirstName))
                .ForMember(t => t.LastName, o => o.MapFrom(t => t.LastName))
                .ForMember(t => t.PersonalNumber, o => o.MapFrom(t => t.PersonalNumber))
                .ForMember(t => t.Email, o => o.MapFrom(t => t.Email))
                .ForMember(t => t.DateOfBirth, o => o.MapFrom(t => t.DateOfBirth));
        }
    }
}
