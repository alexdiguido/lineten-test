using LineTenTest.Api.Controllers;
using System;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace LineTenTest.Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMediator> _mockMediator;

        public OrderControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();
        }

        public OrderController CreateService()
        {
            return new OrderController(_mockMediator.Object);
        }

        [Fact]
        public async Task Get_OrderIdValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();

            // Act
            var result = await orderController.Get(orderId);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.VerifyAll();
        }
    }
}
