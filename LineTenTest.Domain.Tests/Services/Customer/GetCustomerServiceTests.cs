using FluentAssertions;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Customer
{
    public class GetCustomerServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly EfRepository<Domain.Entities.Customer> _customerRepository;

        public GetCustomerServiceTests(DatabaseFixture dbFixture)
        {
            _customerRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
        }

        private GetCustomerService CreateService()
        {
            return new GetCustomerService(_customerRepository);
        }

        [Fact] 
        public async Task GetAsync_CustomerExist_ShouldReturnCustomerAllCustomerData()
        {
            // Arrange
            var service = CreateService();
            int customerId = 1;

            // Act
            var result = await service.GetAsync(customerId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(customerId);
            result.Should().NotBeNull();
            result.FirstName.Should().NotBeNullOrWhiteSpace();
            result.Email.Should().NotBeNullOrWhiteSpace();
            result.LastName.Should().NotBeNullOrWhiteSpace();
            result.Phone.Should().NotBeNullOrWhiteSpace();
        }

        [Fact] 
        public async Task GetAsync_CustomerNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var service = CreateService();
            int customerId = 100;
            string message = "Customer not found";

            // Act
            var result = async () => await service.GetAsync(customerId);

            // Assert

            await result.Should().ThrowAsync<EntityNotFoundException>(message);
        }
    }
}
