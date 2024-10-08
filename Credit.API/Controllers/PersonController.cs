using Credit.Application.Constants.Routing;
using Credit.Application.Handlers.Person.Commands;
using Credit.Application.Handlers.Person.Queries;
using Credit.Application.Models.DtoModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Credit.API.Controllers
{
    [Route(RoutingConstants.Controller)]
    [ApiController]
    public class PersonController : ApiController
    {
        private readonly ISender _mediator;

        public PersonController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(RoutingConstants.Action)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetPersonByIdQuery(id);
            var result = await _mediator.Send(query);

            return Response(result);
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<IActionResult> AddPerson(PersonDtoModel model)
        {
            var command = new AddPersonCommand(model);
            var result = await _mediator.Send(command);

            return Response(result);
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<IActionResult> LoginPerson(LoginPersonDtoModel model)
        {
            var query = new LoginPersonCommandQuery(model);
            var result = await _mediator.Send(query);

            return Response(result);
        }


    }
}
