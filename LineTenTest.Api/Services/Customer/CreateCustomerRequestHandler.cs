using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Services.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Extensions;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services.Customer
{
    public class CreateCustomerRequestHandler : BaseCustomerRequestHandler<CreateCustomerCommand>
    {
        private readonly ICreateCustomerService _service;

        public CreateCustomerRequestHandler(ICreateCustomerService service, ILogger<CreateCustomerRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Customer>> ExecuteServiceOperation(CreateCustomerCommand request)
        {
            Guard.Against.Null(request.Request);
            Guard.Against.NullOrWhiteSpace(request.Request.FirstName);
            Guard.Against.NullOrWhiteSpace(request.Request.LastName);
            Guard.Against.InvalidEmail(request.Request.Email, nameof(request.Request.Email));
            Guard.Against.NullOrWhiteSpace(request.Request.Phone);
            return async () => await _service.CreateAsync(request.Request);
        }
    }
}
