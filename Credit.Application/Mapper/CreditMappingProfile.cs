using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Application.Models.ViewModels;
using Credit.Domain.Entities;

namespace Credit.Application.Mapper
{
    public class CreditMappingProfile : Profile
    {
        public CreditMappingProfile()
        {
            CreateMap<OrderCreditDtoModel, CreditEntity>()
                .ForMember(t => t.RequestAmount, o => o.MapFrom(t => t.RequestAmount))
                .ForMember(t => t.Currency, o => o.MapFrom(t => t.Currency))
                .ForMember(t => t.PeriodStart, o => o.MapFrom(t => t.PeriodStart))
                .ForMember(t => t.PeriodEnd, o => o.MapFrom(t => t.PeriodEnd));

            CreateMap<CreditEntity, CreditViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                .ForMember(t => t.RequestAmount, o => o.MapFrom(t => t.RequestAmount))
                .ForMember(t => t.Currency, o => o.MapFrom(t => t.Currency))
                .ForMember(t => t.PeriodStart, o => o.MapFrom(t => t.PeriodStart))
                .ForMember(t => t.PeriodEnd, o => o.MapFrom(t => t.PeriodEnd))
                .ForMember(t => t.Status, o => o.MapFrom(t => t.Status))
                .ForMember(t => t.PersonId, o => o.MapFrom(t => t.PersonId))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person));

            CreateMap<PersonEntity, CreditPersonViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                .ForMember(t => t.FirstName, o => o.MapFrom(t => t.FirstName))
                .ForMember(t => t.LastName, o => o.MapFrom(t => t.LastName));
        }

    }
}
