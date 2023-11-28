using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Domain.Services.Product;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public class CreateOrderService : ICreateOrderService
{
    private readonly IRepository<Entities.Order> _orderRepository;
    private readonly IRepository<Entities.Customer> _customerRepository;
    private readonly IRepository<Entities.Product> _productRepository;

    public CreateOrderService(IRepository<Entities.Order> orderRepository,
        IRepository<Entities.Customer> customerRepository, 
        IRepository<Entities.Product> productRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<Entities.Order> CreateAsync(CreateOrderRequest createOrderRequest)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(new GetCustomerToUpdateSpecification(createOrderRequest.CustomerId));
        var product =
            await _productRepository.FirstOrDefaultAsync(new GetProductToUpdateSpecification(createOrderRequest.ProductId));
        var orderEntity = OrderFactory.CreateOrder(createOrderRequest);
        orderEntity.CustomerId = customer?.Id ?? throw new EntityNotFoundException("Customer not exists");
        orderEntity.ProductId = product?.Id ?? throw new EntityNotFoundException("Product not exists");;
        orderEntity.Customer = customer;
        orderEntity.Product = product;
        var orderResult = await _orderRepository.AddAsync(orderEntity);
        await _orderRepository.SaveChangesAsync();
        return orderResult;
    }
}