using LineTenTest.Api.Controllers;
using System;
using FluentAssertions;
using Xunit;

namespace LineTenTest.Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task Get_OrderIdValid_ShouldReturnOkActionResult()
        {
            // Arrange
            var orderController = new OrderController();

            // Act
            int orderId = 1;

            var result = await orderController.Get(orderId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
