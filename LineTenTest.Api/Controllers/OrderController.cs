using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Asp.Versioning;
using LineTenTest.Api.ApiModels;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Get([Required]int orderId)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new GetOrderByIdQuery(orderId)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto createOrderDto)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new CreateOrderCommand(createOrderDto)));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Update(UpdateOrderRequest updateOrderRequest)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new UpdateOrderCommand(updateOrderRequest)));
        }

        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Delete([FromQuery]DeleteOrderRequest deleteOrderRequest)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new DeleteOrderCommand(deleteOrderRequest)));
        }

        private async Task<ActionResult<OrderDto>> HandleOrderOperationAsync(
            Func<Task<ActionResult<OrderDto>>> operation)
        {
            try
            {
                return await operation.Invoke();
            }
            catch (Exception ex)
            {
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
