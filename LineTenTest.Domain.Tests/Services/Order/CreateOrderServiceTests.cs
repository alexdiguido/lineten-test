using LineTenTest.Api.Controllers;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.SharedKernel.ApiModels;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Order
{
    public class CreateOrderServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Order> _orderRepository;
        private readonly EfRepository<Domain.Entities.Customer> _customerRepository;
        private readonly EfRepository<Domain.Entities.Product> _productRepository;

        public CreateOrderServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _orderRepository = new EfRepository<Domain.Entities.Order>(dbFixture.DbContext);
            _customerRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
            _productRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private CreateOrderService CreateService()
        {
            return new CreateOrderService(_orderRepository,_customerRepository,_productRepository);
        }

        [Fact] 
        public async Task CreateAsync_OrderExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();

            var customerId = 1;
            var productId = 1;

            var createOrderRequest = new CreateOrderRequest()
            {
                CustomerId = customerId,
                ProductId = productId
            };

            // Act
            var result = await service.CreateAsync(createOrderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Customer.Should().NotBeNull();
            result.CustomerId.Should().Be(customerId);
            result.Customer.Id.Should().Be(customerId);
            result.Product.Should().NotBeNull();
            result.Product.Id.Should().Be(productId);
            result.ProductId.Should().Be(productId);

            _dbFixture.DbContext.Orders.Should()
                .Contain(order => order.Id == result.Id);
        }
    }
}
