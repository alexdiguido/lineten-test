using LineTenTest.Api.Controllers;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel.ApiModels;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Customer
{
    public class DeleteCustomerServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Customer> _customerRepository;

        public DeleteCustomerServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _customerRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
        }

        private DeleteCustomerService CreateService()
        {
            return new DeleteCustomerService(_customerRepository);
        }

        [Fact] 
        public async Task DeleteAsync_CustomerExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var customer = new DeleteCustomerRequest()
            {
                CustomerId = 1
            };

            // Act
            await service.DeleteAsync(customer);
            _dbFixture.DbContext.Customers.Should().NotContain(c => c.Id == customer.CustomerId);
        }

        [Fact] 
        public async Task DeleteAsync_CustomerNotExists_ShouldReturnOrderAllOrderData()
        {
            var service = CreateService();
            int customerId = 100;
            string message = "Customer not found";

            // Act
            var result = async () => await service.DeleteAsync(new DeleteCustomerRequest() {CustomerId = customerId});

            // Assert

            await result.Should().ThrowAsync<NotFoundException>(message);
        }
    }
}
