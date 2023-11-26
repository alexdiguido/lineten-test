using System.Net;
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
    public class GetOrderRequestHandler : BaseOrderRequestHandler<GetOrderByIdQuery>
    {
        private readonly IGetOrderService _service;

        public GetOrderRequestHandler(IGetOrderService service, ILogger<GetOrderRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Order>> ExecuteServiceOperation(GetOrderByIdQuery request)
        {
            Guard.Against.Negative(request.OrderId);
            return async () => await _service.GetAsync(request.OrderId);
        }
    }
}
