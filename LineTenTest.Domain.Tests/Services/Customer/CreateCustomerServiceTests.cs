using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Controllers;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Customer
{
    public class CreateCustomerServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly EfRepository<Domain.Entities.Customer> _orderRepository;

        public CreateCustomerServiceTests(DatabaseFixture dbFixture)
        {
            _orderRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
        }

        private CreateCustomerService CreateService()
        {
            return new CreateCustomerService(_orderRepository);
        }

        [Fact] 
        public async Task CreateAsync_OrderExist_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var email = "Email";
            var phone = "Phone";
            var firstName = "FirstName";
            var lastName = "LastName";
            var customer = new CreateCustomerRequest()
            {
                Email = email,
                Phone = phone,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var result = await service.CreateAsync(customer);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
            result.Phone.Should().Be(phone);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
        }
    }
}
