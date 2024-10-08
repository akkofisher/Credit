using Credit.Application.Constants.Routing;
using Credit.Application.Handlers.Credit.Commands;
using Credit.Application.Handlers.Credit.Queries;
using Credit.Application.Models.DtoModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit.API.Controllers
{
    [Authorize]
    [Route(RoutingConstants.Controller)]
    [ApiController]
    public class CreditController : ApiController
    {
        private readonly ISender _mediator;

        public CreditController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(RoutingConstants.Action)]
        public async Task<IActionResult> GetPersonOrderCredits()
        {
            var query = new GetPersonCreditsQuery();
            var result = await _mediator.Send(query);

            return Response(result);
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<IActionResult> OrderCredit(OrderCreditDtoModel model)
        {
            var command = new OrderCreditCommand(model);
            var result = await _mediator.Send(command);

            return Response(result);
        }

        [HttpPut(RoutingConstants.Action)]
        public async Task<IActionResult> EditCredit(OrderCreditDtoModel model)
        {
            var command = new EditCreditCommand(model);
            var result = await _mediator.Send(command);

            return Response(result);
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<IActionResult> SendCredit(int id)
        {
            var command = new ChangeOrderCreditStatusCommand(id);
            var result = await _mediator.Send(command);

            return Response(result);
        }

        [HttpDelete(RoutingConstants.Action)]
        public async Task<IActionResult> DeleteCredit(int id)
        {
            var command = new DeleteOrderCreditCommand(id);
            var result = await _mediator.Send(command);

            return Response(result);
        }

    }
}