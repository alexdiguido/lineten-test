﻿using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Services.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotFoundException = LineTenTest.Domain.Exceptions.NotFoundException;

namespace LineTenTest.Api.Services
{
    public class GetOrderRequestHandler : IRequestHandler<GetOrderByIdQuery,ActionResult<OrderDto>>
    {
        private readonly IGetOrderService _service;
        private readonly ILogger<GetOrderRequestHandler> _logger;

        public GetOrderRequestHandler(IGetOrderService service, ILogger<GetOrderRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<ActionResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            Order? orderResult;
            try
            {
                Guard.Against.Negative(request.OrderId);
                orderResult = await _service.GetAsync(request.OrderId);
            }
            catch (ArgumentException argumentEx)
            {
                return new BadRequestObjectResult(argumentEx.Message);
            }
            catch (NotFoundException ex)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,request.OrderId);
                return new ObjectResult(Constants.InternalServerErrorResultMessage)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            
            return new OkObjectResult(OrderDtoMapper.MapFrom(orderResult));
        }
    }
}
