using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Customer
{
    public class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerCommand,ActionResult<CustomerDto>>
    {
        public CreateCustomerRequestHandler()
        {
            
        }

        public Task<ActionResult<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
