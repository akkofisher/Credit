using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Application.Settings.APIAuthorization;
using Credit.Application.Settings.RabbitMQ;
using Credit.Domain.Entities;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.Credit.Commands
{
    public class OrderCreditCommand(OrderCreditDtoModel model) : IRequest<int>
    {
        public OrderCreditDtoModel Model { get; } = model;
    }

    public class OrderCreditCommandHandler : IRequestHandler<OrderCreditCommand, int>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;
        private readonly IRabbitMQService _rabbitMQProducer;

        public OrderCreditCommandHandler(IUnitOfWork unitOfWork,
            IPersonRepository personRepository,
            IMapper mapper,
            IApiAuthorization apiAuthorization,
            ICreditRepository creditRepository, IRabbitMQService rabbitMqProducer)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiAuthorization = apiAuthorization;
            _creditRepository = creditRepository;
            _rabbitMQProducer = rabbitMqProducer;
        }

        public async Task<int> Handle(OrderCreditCommand request,
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

            //create credit order
            var credit = _mapper.Map<CreditEntity>(request.Model);
            credit.PersonId = person.Id;
            credit.Status = CreditStatusEnum.OnEdit;

            await _creditRepository.Add(credit);
            await _unitOfWork.CommitAsync();

            return credit.Id;
        }

    }
}
