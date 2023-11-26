using LineTenTest.Domain.Entities;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Api.Services;

public interface ICreateOrderService
{
    Task<Order> CreateAsync(CreateOrderRequest request);
}