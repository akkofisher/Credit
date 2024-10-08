using AutoMapper;
using Credit.Application.Models.ViewModels;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Person;
using MediatR;

namespace Credit.Application.Handlers.Person.Queries
{
    public class GetPersonByIdQuery(int id) : IRequest<PersonViewModel>
    {
        public int Id { get; } = id;
    }


    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonViewModel>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //constructor
        public GetPersonByIdQueryHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PersonViewModel> Handle(GetPersonByIdQuery request,
            CancellationToken cancellationToken)
        {

            var person = await _personRepository.GetById(request.Id);

            if (person == null)
            {
                throw new Exception("Person not found");
            }

            var personData = _mapper.Map<PersonViewModel>(person);

            return personData;
        }

    }
}
