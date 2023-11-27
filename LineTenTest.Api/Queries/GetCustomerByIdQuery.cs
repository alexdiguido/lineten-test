using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Queries;

public class GetCustomerByIdQuery : IRequest<ActionResult<CustomerDto>>
{
    public int CustomerId { get; set; }

    public GetCustomerByIdQuery(int customerId)
    {
        CustomerId = customerId;
    }
}