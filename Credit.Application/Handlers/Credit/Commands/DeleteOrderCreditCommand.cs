﻿using AutoMapper;
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
    public class DeleteOrderCreditCommand(int id) : IRequest<int>
    {
        public int Id { get; } = id;
    }

    public class DeleteOrderCreditCommandHandler : IRequestHandler<DeleteOrderCreditCommand, int>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiAuthorization _apiAuthorization;
        private readonly IRabbitMQService _rabbitMQProducer;

        public DeleteOrderCreditCommandHandler(IUnitOfWork unitOfWork,
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

        public async Task<int> Handle(DeleteOrderCreditCommand request,
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

            if (credit.Status != CreditStatusEnum.OnEdit)
            {
                throw new Exception("Credit Order status is not On Edit");
            }

            _creditRepository.DeleteSoft(credit.Id);
            await _unitOfWork.CommitAsync();

            return credit.Id;
        }

    }
}
