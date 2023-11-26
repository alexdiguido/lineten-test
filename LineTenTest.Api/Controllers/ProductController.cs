﻿using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Commands;
using LineTenTest.SharedKernel.ApiModels;
using System.Net.Mime;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace LineTenTest.Api.Controllers
{
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
        public async Task<ActionResult<OrderDto>> Get([Required]int productId)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new GetProductByIdQuery(productId)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Create(CreateProductRequest createOrderRequest)
        {
            throw new NotImplementedException();
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
                _logger.LogError(ex,ex.Message);
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}