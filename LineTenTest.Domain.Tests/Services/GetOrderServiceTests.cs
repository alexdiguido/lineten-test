using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Services;
using LineTenTest.SharedKernel;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using LineTenTest.Infrastructure;

namespace LineTenTest.Domain.Tests.Services
{
    public class GetOrderServiceTests
    {
        private readonly EfRepository<Order> _orderRepository;

        public GetOrderServiceTests()
        {
            var dbContext = SetupOrderContext();
            _orderRepository = new EfRepository<Order>(dbContext);
        }

        private AppDbContext SetupOrderContext()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connectionString)
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private GetOrderService CreateService()
        {
            return new GetOrderService(_orderRepository);
        }

        [Fact]
        public async Task GetAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            int orderId = 1;

            // Act
            var result = await service.GetAsync(
                orderId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
