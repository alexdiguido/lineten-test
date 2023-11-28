using Ardalis.GuardClauses;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Utilities.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LineTenTest.Api.Extensions;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services.Customer
{
    public class GetCustomerRequestHandler : BaseCustomerRequestHandler<GetCustomerByIdQuery>
    {
        private readonly IGetCustomerService _service;

        public GetCustomerRequestHandler(IGetCustomerService service, ILogger<GetCustomerRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Customer>> ExecuteServiceOperation(GetCustomerByIdQuery request)
        {
            Guard.Against.Negative(request.CustomerId);
            return async () => await _service.GetAsync(request.CustomerId);
        }
    }
}
