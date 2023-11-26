using LineTenTest.Domain.Services.Order;
using Moq;
using System;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentAssertions;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Services.Order;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Exceptions;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq.AutoMock;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Order
{
    public class GetOrderRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private GetOrderRequestHandler CreateGetOrderRequestHandler()
        {
            return _mockRepository.CreateInstance<GetOrderRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var getOrderRequestHandler = CreateGetOrderRequestHandler();
            int orderId = 1;
            GetOrderByIdQuery request = new GetOrderByIdQuery(orderId);
            CancellationToken cancellationToken = default;
            Domain.Entities.Order orderEntity = OrderBuilder.CreateDefault();

            _mockRepository.GetMock<IGetOrderService>().Setup(s => s.GetAsync(orderId))
                .ReturnsAsync(orderEntity);

            // Act
            var result = await getOrderRequestHandler.Handle(request, cancellationToken);

            // Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
            var value = (OrderDto)objectResult.Value!;
            value.ShouldBeEquivalentTo(orderEntity);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsValid_OrderNotFound_ShouldReturnNotFoundResult()
        {
            // Arrange
            var getOrderRequestHandler = CreateGetOrderRequestHandler();
            int orderId = 1;
            GetOrderByIdQuery request = new GetOrderByIdQuery(orderId);
            CancellationToken cancellationToken = default;

            var exceptionMessage = "message";
            _mockRepository.GetMock<IGetOrderService>().Setup(s => s.GetAsync(orderId))
                .ThrowsAsync(new NotFoundException(exceptionMessage));

            // Act
            var result = await getOrderRequestHandler.Handle(request, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceThrowException_ShouldReturnInternalServerErrorResult()
        {
            // Arrange
            var getOrderRequestHandler = CreateGetOrderRequestHandler();
            int orderId = 1;
            GetOrderByIdQuery request = new GetOrderByIdQuery(orderId);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IGetOrderService>().Setup(s => s.GetAsync(orderId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await getOrderRequestHandler.Handle(request, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);
            objectResult.Value.Should().Be(Constants.InternalServerErrorResultMessage);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult()
        {
            // Arrange
            var getOrderRequestHandler = CreateGetOrderRequestHandler();
            int orderId = -1;
            GetOrderByIdQuery request = new GetOrderByIdQuery(orderId);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await getOrderRequestHandler.Handle(request, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
