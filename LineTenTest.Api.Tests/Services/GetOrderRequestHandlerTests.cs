using LineTenTest.Api.Services;
using LineTenTest.Domain.Services.Order;
using Moq;
using System;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentAssertions;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Entities;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LineTenTest.Api.Tests.Services
{
    public class GetOrderRequestHandlerTests
    {
        private MockRepository _mockRepository;
        private Mock<IGetOrderService> _mockGetOrderService;

        public GetOrderRequestHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockGetOrderService = this._mockRepository.Create<IGetOrderService>();
        }

        private GetOrderRequestHandler CreateGetOrderRequestHandler()
        {
            return new GetOrderRequestHandler(_mockGetOrderService.Object);
        }

        [Fact] public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var getOrderRequestHandler = CreateGetOrderRequestHandler();
            int orderId = 1;
            GetOrderByIdQuery request = new GetOrderByIdQuery(orderId);
            CancellationToken cancellationToken = default;
            Order orderEntity = OrderBuilder.CreateDefault();

            _mockGetOrderService.Setup(s => s.GetAsync(orderId))
                .ReturnsAsync(orderEntity);

            // Act
            var result = await getOrderRequestHandler.Handle(request, cancellationToken);

            // Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            var value = (OrderDto)objectResult.Value;
            value.ShouldBeEquivalentTo(orderEntity);

            _mockRepository.VerifyAll();
        }
    }
}
