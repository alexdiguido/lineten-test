﻿using LineTenTest.Api.ApiModels;
using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class DeleteOrderCommand : IRequest<ActionResult<OrderDto>>
{
    public DeleteOrderRequest Request { get; }

    public DeleteOrderCommand(DeleteOrderRequest request)
    {
        Request = request;
    }
}