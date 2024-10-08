using Credit.Application.Constants.Routing;
using Credit.Application.Handlers.AdminCredit.Commands;
using Credit.Application.Handlers.AdminCredit.Queries;
using Credit.Application.Models.CommonModels;
using Credit.Application.Models.DtoModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit.API.Controllers
{
    [Authorize]
    [Route(RoutingConstants.Controller)]
    [ApiController]
    public class AdminCreditController : ApiController
    {
        private readonly ISender _mediator;

        public AdminCreditController(ISender mediator)
        {
            _mediator = mediator;
        }


        [HttpGet(RoutingConstants.Action)]
        public async Task<IActionResult> GetOrdersCreditsFromQueue()
        {
            var query = new GetOrdersCreditsFromQueueQuery();
            var result = await _mediator.Send(query);

            return Response(result);
        }

        [HttpGet(RoutingConstants.Action)]
        public async Task<IActionResult> GetCredits([FromQuery] PagingModel model)
        {
            var query = new GetCreditsQueryQuery(model);
            var result = await _mediator.Send(query);

            return Response(result);
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<IActionResult> ChangeOrderCreditStatus(ChangeOrderCreditStatusDtoModel model)
        {
            var command = new ChangeOrderCreditStatusCommand(model);
            var result = await _mediator.Send(command);

            return Response(result);
        }

    }
}