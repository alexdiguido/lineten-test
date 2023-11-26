using FluentAssertions;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Domain.Tests.Entities
{
    public class OrderTests
    {
        [Fact]
        public void Order_Constructor_PropertiseShouldContainsDefaultValues()
        {
            var datetime = new DateTime();
            var order = new Order();

            order.Should().NotBeNull();
            order.Product.Should().NotBeNull();
            order.Customer.Should().NotBeNull();
            order.CreatedDate.Should().Be(datetime);
            order.UpdatedDate.Should().Be(datetime);
        }

        [Fact]
        public void Order_Constructor_PropertiesAssignedWithValue_PropertiesShouldContainsAssignedValues()
        {
            var createdDateTime = new DateTime();
            var updatedDateTime = createdDateTime.AddHours(1);
            var status = (int)EOrderStatus.Pending;
            var customer = new Customer();
            var product = new Product();
            var order = new Order()
            {
                Id = 1,
                Customer = customer,
                Product = product,
                UpdatedDate = updatedDateTime,
                CreatedDate = createdDateTime,
                Status = status
            };

            order.Should().NotBeNull();
            order.Product.Should().Be(product);
            order.Customer.Should().Be(customer);
            order.Status.Should().Be(status);
            order.CreatedDate.Should().Be(createdDateTime);
            order.UpdatedDate.Should().Be(updatedDateTime);
        }
    }
}