using LineTenTest.Api.Commands;
using LineTenTest.Domain.Services.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Customer
{
    public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerCommand,IActionResult>
    {
        public DeleteCustomerRequestHandler(IDeleteCustomerService service, ILogger<DeleteCustomerRequestHandler> logger)
        {
            
        }
        public Task<IActionResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
