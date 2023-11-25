using LineTenTest.Domain.Entities;
using LineTenTest.SharedKernel;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using LineTenTest.Infrastructure;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Domain.Exceptions;

namespace LineTenTest.Domain.Tests.Services
{
    public class GetOrderServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly EfRepository<Order> _orderRepository;

        public GetOrderServiceTests(DatabaseFixture dbFixture)
        {
            _orderRepository = new EfRepository<Order>(dbFixture.DbContext);
        }

        private GetOrderService CreateService()
        {
            return new GetOrderService(_orderRepository);
        }

        [Fact] 
        public async Task GetAsync_OrderExist_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            int orderId = 1;

            // Act
            var result = await service.GetAsync(orderId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
            result.Customer.Should().NotBeNull();
            result.Customer.FirstName.Should().NotBeNullOrWhiteSpace();
            result.Customer.Email.Should().NotBeNullOrWhiteSpace();
            result.Customer.LastName.Should().NotBeNullOrWhiteSpace();
            result.Customer.Phone.Should().NotBeNullOrWhiteSpace();
            result.Product.Should().NotBeNull();
            result.Product.Description.Should().NotBeNullOrWhiteSpace();
            result.Product.Name.Should().NotBeNullOrWhiteSpace();
            result.Product.SKU.Should().NotBeNullOrWhiteSpace();
        }

        [Fact] 
        public async Task GetAsync_OrderNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var service = CreateService();
            int orderId = 100;
            string message = "Order not found";

            // Act
            var result = async () => await service.GetAsync(orderId);

            // Assert

            await result.Should().ThrowAsync<NotFoundException>(message);
        }
    }
}
