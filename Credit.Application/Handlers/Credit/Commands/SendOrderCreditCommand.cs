using AutoMapper;
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
    public class ChangeOrderCreditStatusCommand(int id) : IRequest<int>
    {
        public int Id { get; } = id;
    }

    public class SendOrderCreditCommandHandler : IRequestHandler<ChangeOrderCreditStatusCommand, int>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;
        private readonly IRabbitMQService _rabbitMQProducer;

        public SendOrderCreditCommandHandler(IUnitOfWork unitOfWork,
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

        public async Task<int> Handle(ChangeOrderCreditStatusCommand request,
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

            //get credit details
            var credit = await _creditRepository.GetCreditByIdAndPersonId(request.Id, person.Id);

            if (credit == null)
            {
                throw new Exception("Credit not found");
            }

            credit.Status = CreditStatusEnum.Pending;

            _creditRepository.Update(credit);
            await _unitOfWork.CommitAsync();

            //Publish order credit to RabbitMQ
            _rabbitMQProducer.BasicPublish(credit);

            return credit.Id;
        }

    }
}
