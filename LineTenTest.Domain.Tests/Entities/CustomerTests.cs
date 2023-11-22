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
    }
}
