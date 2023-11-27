using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Commands;
using LineTenTest.SharedKernel.ApiModels;
using System.Net.Mime;
using Microsoft.AspNetCore.Server.IIS.Core;
using LineTenTest.Domain.Entities;
using Asp.Versioning;

namespace LineTenTest.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> Get([Required]int productId)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new GetProductByIdQuery(productId)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> Create(CreateProductRequest createOrderRequest)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new CreateProductCommand(createOrderRequest)));
        }

        
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> Update(UpdateProductRequest updateOrderRequest)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new UpdateProductCommand(updateOrderRequest)));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery]DeleteProductRequest deleteProductRequest)
        {
            try
            {
                return await _mediator.Send(new DeleteProductCommand(deleteProductRequest));
            }
            catch (Exception ex)
            {
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        private async Task<ActionResult<ProductDto>> HandleOrderOperationAsync(
            Func<Task<ActionResult<ProductDto>>> operation)
        {
            try
            {
                return await operation.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
