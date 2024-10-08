using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Application.Settings.APIAuthorization;
using Credit.Application.Settings.RabbitMQ;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.AdminCredit.Commands
{
    public class ChangeOrderCreditStatusCommand(ChangeOrderCreditStatusDtoModel model) : IRequest<int>
    {
        public ChangeOrderCreditStatusDtoModel Model { get; } = model;
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
            if (_apiAuthorization.GetAuthorizeRole() != RoleEnum.Admin)
            {
                throw new Exception("Role is not Person");
            }

            //get credit details
            var credit = await _creditRepository.GetCreditById(request.Model.Id);

            if (credit == null)
            {
                throw new Exception("Credit not found");
            }

            credit.Status = request.Model.Status;

            _creditRepository.Update(credit);
            await _unitOfWork.CommitAsync();

            return credit.Id;
        }

    }
}
