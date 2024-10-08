using AutoMapper;
using Credit.Application.Models.ViewModels;
using Credit.Application.Settings.APIAuthorization;
using Credit.Application.Settings.RabbitMQ;
using Credit.Domain.Entities;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.AdminCredit.Queries
{
    public class GetOrdersCreditsFromQueueQuery() : IRequest<List<CreditViewModel>>
    {
    }

    public class GetOrdersCreditsFromQueueQueryHandler : IRequestHandler<GetOrdersCreditsFromQueueQuery, List<CreditViewModel>>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;

        //constructor
        public GetOrdersCreditsFromQueueQueryHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper, IApiAuthorization apiAuthorization, ICreditRepository creditRepository, IRabbitMQService rabbitMqService)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiAuthorization = apiAuthorization;
            _creditRepository = creditRepository;
            _rabbitMQService = rabbitMqService;
        }

        public async Task<List<CreditViewModel>> Handle(GetOrdersCreditsFromQueueQuery request,
            CancellationToken cancellationToken)
        {
            if (_apiAuthorization.GetAuthorizeId() == null)
            {
                throw new Exception("Unauthorized");
            }

            //check if role is admin
            if (_apiAuthorization.GetAuthorizeRole() != RoleEnum.Admin)
            {
                throw new Exception("Role is not Admin");
            }

            //get credits from rabbit mq queue // first 10 
            var credits = _rabbitMQService.BasicGet<CreditEntity>();

            //get all person ids
            var personIds = credits.Select(x => x.PersonId).Distinct().ToList();

            var getPersonNames = await _personRepository.GetPersonNamesByIds(personIds);

            //map person names to credits
            foreach (var credit in credits)
            {
                credit.Person = getPersonNames.FirstOrDefault(x => x.Id == credit.PersonId);
            }

            //map to view model by mapper
            var creditsData = _mapper.Map<List<CreditViewModel>>(credits);

            return creditsData;
        }

    }
}