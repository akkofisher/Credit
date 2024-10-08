using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Domain.Entities;
using Credit.Domain.Enums;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.Person.Commands
{
    public class AddPersonCommand(PersonDtoModel model) : IRequest<int>
    {
        public PersonDtoModel Model { get; } = model;
    }

    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, int>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddPersonCommandHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddPersonCommand request,
            CancellationToken cancellationToken)
        {
            //check if person with same personal number or email exist
            var emailExist = await _personRepository.GetByEmail(request.Model.Email);

            if (emailExist != null)
            {
                throw new Exception("Person with same email already exists");
            }
            var personalNumberExist = await _personRepository.GetByPersonalNumber(request.Model.PersonalNumber);

            if (personalNumberExist != null)
            {
                throw new Exception("Person with same personal number already exists");
            }

            var person = _mapper.Map<PersonEntity>(request.Model);
            person.Role = RoleEnum.Person;

            // Encrypt password
            person.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Model.Password);

            //save
            await _personRepository.Add(person);

            await _unitOfWork.CommitAsync();

            return person.Id;
        }

    }
}
