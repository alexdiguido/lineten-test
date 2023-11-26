using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services
{
    public class CreateOrderRequestHandler : BaseOrderRequestHandler<CreateOrderCommand>
    {
        private readonly ICreateOrderService _service;

        public CreateOrderRequestHandler(ICreateOrderService service, ILogger<CreateOrderRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Order>> ExecuteServiceOperation(CreateOrderCommand request)
        {
            Guard.Against.Null(request.CreateOrderRequest);
            Guard.Against.Negative(request.CreateOrderRequest.CustomerId);
            Guard.Against.Negative(request.CreateOrderRequest.ProductId);
            return async() => await _service.CreateAsync(request.CreateOrderRequest);
        }
    }
}
