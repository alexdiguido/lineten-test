using LineTenTest.Domain.Services.Order;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel.ApiModels;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Order
{
    public class UpdateOrderServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Order> _orderRepository;
        private readonly EfRepository<Domain.Entities.Customer> _customerRepository;
        private readonly EfRepository<Domain.Entities.Product> _productRepository;

        public UpdateOrderServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _orderRepository = new EfRepository<Domain.Entities.Order>(dbFixture.DbContext);
            _customerRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
            _productRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private UpdateOrderService UpdateService()
        {
            return new UpdateOrderService(_orderRepository,_customerRepository,_productRepository);
        }

        [Fact] 
        public async Task UpdateAsync_OrderExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = UpdateService();

            var customerId = 2;
            var productId = 2;
            var orderId = 1;
            var newStatus = 5;

            var createOrderRequest = new UpdateOrderRequest()
            {
                OrderId = orderId,
                Status = newStatus,
                CustomerId = customerId,
                ProductId = productId
            };

            // Act
            var result = await service.UpdateAsync(createOrderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Customer.Should().NotBeNull();
            result.CustomerId.Should().Be(customerId);
            result.Customer.Id.Should().Be(customerId);
            result.Product.Should().NotBeNull();
            result.Product.Id.Should().Be(productId);
            result.ProductId.Should().Be(productId);
            result.Status.Should().Be(newStatus);

            _dbFixture.DbContext.Orders.Should()
                .Contain(order => order.Id == result.Id && 
                                  order.Customer.Id == customerId &&
                                  order.Product.Id == productId &&
                                  order.Status == newStatus);
        }

        [Theory]
        [InlineData(11111,1,1, "Order Not Found")]
        [InlineData(1,111111,1, "Customer not found")]
        [InlineData(1,1,1111111, "Product not found")]
        public async Task UpdateAsync_RequestNotValid_ShouldReturnOrderAllOrderData(int orderId, int productId, int customerId, string expectedMessage)
        {
            // Arrange
            var service = UpdateService();

            var newStatus = 5;

            var updateOrderRequest = new UpdateOrderRequest()
            {
                OrderId = orderId,
                Status = newStatus,
                CustomerId = customerId,
                ProductId = productId
            };

            // Act
            var result = async () => await service.UpdateAsync(updateOrderRequest);

            // Assert

            await result.Should().ThrowAsync<EntityNotFoundException>(expectedMessage);
        }
    }
}
