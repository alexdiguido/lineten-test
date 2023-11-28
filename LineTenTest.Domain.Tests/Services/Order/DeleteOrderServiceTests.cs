using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Order
{
    public class DeleteOrderServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Order> _customerRepository;

        public DeleteOrderServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _customerRepository = new EfRepository<Domain.Entities.Order>(dbFixture.DbContext);
        }

        private DeleteOrderService CreateService()
        {
            return new DeleteOrderService(_customerRepository);
        }

        [Fact] 
        public async Task DeleteAsync_OrderExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var customer = new DeleteOrderRequest()
            {
                OrderId = 1
            };

            // Act
            await service.DeleteAsync(customer);
            _dbFixture.DbContext.Orders.Should().NotContain(c => c.Id == customer.OrderId);
        }

        [Fact] 
        public async Task DeleteAsync_OrderNotExists_ShouldReturnOrderAllOrderData()
        {
            var service = CreateService();
            int customerId = 100;
            string message = "Order not found";

            // Act
            var result = async () => await service.DeleteAsync(new DeleteOrderRequest() {OrderId = customerId});

            // Assert

            await result.Should().ThrowAsync<NotFoundException>(message);
        }
    }
}
