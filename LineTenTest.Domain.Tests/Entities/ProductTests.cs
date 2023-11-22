using FluentAssertions;
using LineTenTest.Domain.Entities;
using System;
using Xunit;

namespace LineTenTest.Domain.Tests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void DefaultConstructor_PropertiesShouldNotBeNull()
        {
            // Act
            var product = new Product();

            // Assert
            product.Description.Should().NotBeNull();
            product.Name.Should().NotBeNull();
            product.SKU.Should().NotBeNull();
        }

        [Fact]
        public void DefaultConstructor_WithPropertiesAssigned_PropertiesShouldReturnAssignedValues()
        {
            // Arrange
            var id = 1;
            var description = "description";
            var name = "name";
            var sku = "sku";
            
            //Act
            var product = new Product()
            {
                Description = description,
                Name = name,
                SKU = sku,
                Id = id,
            };

            // Assert
            product.Description.Should().Be(description);
            product.Name.Should().Be(name);
            product.SKU.Should().Be(sku);
            product.Id.Should().Be(id);
        }
    }
}
