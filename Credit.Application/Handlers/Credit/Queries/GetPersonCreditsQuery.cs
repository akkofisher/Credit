using AutoMapper;
using Credit.Application.Models.ViewModels;
using Credit.Application.Settings.APIAuthorization;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.Credit.Queries
{
    public class GetPersonCreditsQuery() : IRequest<List<CreditViewModel>>
    {
    }


    public class GetPersonQueryHandler : IRequestHandler<GetPersonCreditsQuery, List<CreditViewModel>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;

        //constructor
        public GetPersonQueryHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper, IApiAuthorization apiAuthorization, ICreditRepository creditRepository)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiAuthorization = apiAuthorization;
            _creditRepository = creditRepository;
        }

        public async Task<List<CreditViewModel>> Handle(GetPersonCreditsQuery request,
            CancellationToken cancellationToken)
        {
            if (_apiAuthorization.GetAuthorizeId() == null)
            {
                throw new Exception("Unauthorized");
            }

            //check if role is person
            if (_apiAuthorization.GetAuthorizeRole() != RoleEnum.Person)
            {
                throw new Exception("Role is not Person");
            }

            //get person details
            var person = await _personRepository.GetById(_apiAuthorization.GetAuthorizeId().Value);

            if (person == null)
            {
                throw new Exception("Person not found");
            }

            //get person  credits
            var personCredits = await _creditRepository.GetCreditsByPersonId(person.Id);

            //map to view model by mapper
            var creditsData = _mapper.Map<List<CreditViewModel>>(personCredits.ToList());

            return creditsData;
        }

    }
}