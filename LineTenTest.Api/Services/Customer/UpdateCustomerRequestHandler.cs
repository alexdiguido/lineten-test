using LineTenTest.Api.Commands;
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
        throw new NotImplementedException();
    }
}