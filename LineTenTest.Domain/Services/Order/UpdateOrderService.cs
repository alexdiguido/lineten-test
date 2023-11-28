using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Domain.Services.Product;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public class UpdateOrderService : IUpdateOrderService
{
    private readonly IRepository<Entities.Customer> _customerRepository;
    private readonly IRepository<Entities.Order> _orderRepository;
    private readonly IRepository<Entities.Product> _productRepository;

    public UpdateOrderService(IRepository<Entities.Order> orderRepository,
        IRepository<Entities.Customer> customerRepository, IRepository<Entities.Product> productRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Entities.Order> UpdateAsync(UpdateOrderRequest request)
    {
        var order = await _orderRepository.FirstOrDefaultAsync(new GetOrderByIdSpecificationToUpdate(request.OrderId));
        if (order == null)
        {
            throw new NotFoundException("Order Not Found");
        }

        if (order.Customer.Id != request.CustomerId)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(new GetCustomerToUpdateSpecification(request.CustomerId));
            order.CustomerId = customer?.Id ?? throw new NotFoundException("Customer not found");
            order.Customer = customer;
        }

        if (order.Product.Id != request.ProductId)
        {
            var product =
                await _productRepository.FirstOrDefaultAsync(new GetProductToUpdateSpecification(request.ProductId));
            order.ProductId = product?.Id ?? throw new NotFoundException("Product not found");;
            order.Product = product;
        }
        
        order.Status = request.Status;
        await _orderRepository.SaveChangesAsync();

        return order;
    }
}