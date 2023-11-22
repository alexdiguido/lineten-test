using LineTenTest.Domain.Entities;
using System;
using FluentAssertions;
using Xunit;

namespace LineTenTest.Domain.Tests.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void DefaultConstructor_PropertiesShouldNotBeNull()
        {
            // Act
            var customer = new Customer();

            // Assert
            customer.Email.Should().NotBeNull();
            customer.FirstName.Should().NotBeNull();
            customer.LastName.Should().NotBeNull();
            customer.Phone.Should().NotBeNull();
        }

        [Fact]
        public void DefaultConstructor_WithPropertiesAssigned_PropertiesShouldReturnAssignedValues()
        {
            // Arrange
            var email = "email";
            var firstName = "firstName";
            var lastName = "lastName";
            var phone = "phone";
            var id = 1;

            //Act
            var customer = new Customer()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                Phone = phone
            };

            // Assert
            customer.Email.Should().Be(email);
            customer.FirstName.Should().Be(firstName);
            customer.LastName.Should().Be(lastName);
            customer.Phone.Should().Be(phone);
            customer.Id.Should().Be(id);
        }
    }
}
