using AutoMapper;
using Credit.Application.Models.CommonModels;
using Credit.Application.Models.ViewModels;
using Credit.Application.Settings.APIAuthorization;
using Credit.Domain.Entities;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.AdminCredit.Queries
{
    public class GetCreditsQueryQuery(PagingModel model) : IRequest<List<CreditViewModel>>
    {
        public PagingModel Model = model;
    }


    public class GetCreditsQueryHandler : IRequestHandler<GetCreditsQueryQuery, List<CreditViewModel>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;

        //constructor
        public GetCreditsQueryHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper, IApiAuthorization apiAuthorization, ICreditRepository creditRepository)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiAuthorization = apiAuthorization;
            _creditRepository = creditRepository;
        }

        public async Task<List<CreditViewModel>> Handle(GetCreditsQueryQuery request,
            CancellationToken cancellationToken)
        {
            if (_apiAuthorization.GetAuthorizeId() == null)
            {
                throw new Exception("Unauthorized");
            }

            //check if role is person
            if (_apiAuthorization.GetAuthorizeRole() != RoleEnum.Admin)
            {
                throw new Exception("Role is not Admin");
            }

            //get person order credits
            //include person details
            //only status not equal to OnEdit
            var personCredits = await _creditRepository.Get(
                filters: x => x.Status != CreditStatusEnum.OnEdit,
                includes:
                [
                    x => x.Person,
                ],
                skip: request.Model.Start * request.Model.Page,
                take: request.Model.Limit
                );

            //map to view model by mapper
            var creditsData = _mapper.Map<List<CreditViewModel>>(personCredits.ToList());

            return creditsData;
        }

    }
}