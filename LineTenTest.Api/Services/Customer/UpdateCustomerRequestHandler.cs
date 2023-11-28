using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Extensions;
using LineTenTest.Domain.Services.Customer;

namespace LineTenTest.Api.Services.Customer;

public class UpdateCustomerRequestHandler : BaseCustomerRequestHandler<UpdateCustomerCommand>
{
    private readonly IUpdateCustomerService _service;

    public UpdateCustomerRequestHandler(IUpdateCustomerService service, ILogger<BaseCustomerRequestHandler<UpdateCustomerCommand>> logger) : base(logger)
    {
        _service = service;
    }

    protected override Func<Task<Domain.Entities.Customer>> ExecuteServiceOperation(UpdateCustomerCommand request)
    {
        Guard.Against.Null(request.Request);
        Guard.Against.Negative(request.Request.CustomerId);
        Guard.Against.NullOrWhiteSpace(request.Request.FirstName);
        Guard.Against.NullOrWhiteSpace(request.Request.LastName);
        Guard.Against.InvalidEmail(request.Request.Email, nameof(request.Request.Email));
        Guard.Against.NullOrWhiteSpace(request.Request.Phone);
        return async () => await _service.UpdateAsync(request.Request);
    }
}