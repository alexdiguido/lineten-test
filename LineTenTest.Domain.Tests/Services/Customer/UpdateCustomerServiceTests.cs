using LineTenTest.Domain.Services.Customer;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel.ApiModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Customer
{
    public class UpdateCustomerServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Customer> _customerRepository;

        public UpdateCustomerServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _customerRepository = new EfRepository<Domain.Entities.Customer>(dbFixture.DbContext);
        }

        private UpdateCustomerService CreateService()
        {
            return new UpdateCustomerService(_customerRepository);
        }

        [Fact] 
        public async Task UpdateAsync_CustomerExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var email = "Emailupdated";
            var phone = "Phoneupdated";
            var firstName = "FirstNameupdated";
            var lastName = "LastNameupdated";
            var customer = new UpdateCustomerRequest()
            {
                CustomerId = 1,
                Email = email,
                Phone = phone,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var result = await service.UpdateAsync(customer);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
            result.Phone.Should().Be(phone);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
            _dbFixture.DbContext.Customers.Should().Contain(customer => customer.Email == email &&
                                                                        customer.FirstName == firstName &&
                                                                        customer.LastName == lastName &&
                                                                        customer.Phone == phone);
        }

        [Fact] 
        public async Task UpdateAsync_CustomerNotExists_ShouldReturnNotFoundException()
        {
            // Arrange
            var service = CreateService();
            var email = "Emailupdated";
            var phone = "Phoneupdated";
            var firstName = "FirstNameupdated";
            var lastName = "LastNameupdated";
            var customer = new UpdateCustomerRequest()
            {
                CustomerId = 223, // id should not exist in the dbContext
                Email = email,
                Phone = phone,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var result = async () => await service.UpdateAsync(customer);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}
