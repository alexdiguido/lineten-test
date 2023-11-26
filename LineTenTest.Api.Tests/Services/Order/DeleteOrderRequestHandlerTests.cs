using LineTenTest.Api.Services.Order;
using LineTenTest.Domain.Services.Order;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Tests.Services.Order
{
    public class DeleteOrderRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private DeleteOrderRequestHandler CreateDeleteOrderRequestHandler()
        {
            return _mockRepository.CreateInstance<DeleteOrderRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOk_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var deleteOrderRequestHandler = CreateDeleteOrderRequestHandler();

            var request = new DeleteOrderRequest() { OrderId = 1};

            var command = new DeleteOrderCommand(request);
            CancellationToken cancellationToken = default;
            Domain.Entities.Order orderEntity = OrderBuilder.CreateDefault();

            _mockRepository.GetMock<IDeleteOrderService>().Setup(s => s.DeleteAsync(request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await deleteOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            var objectResult = result as OkResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceThrowException_ShouldReturnInternalServerErrorResult()
        {
            // Arrange
            var deleteOrderRequestHandler = CreateDeleteOrderRequestHandler();

            var request = new DeleteOrderRequest() { OrderId = 1};

            var command = new DeleteOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IDeleteOrderService>().Setup(s => s.DeleteAsync(It.IsAny<DeleteOrderRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await deleteOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);
            objectResult.Value.Should().Be(Constants.InternalServerErrorResultMessage);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult()
        {
            // Arrange
            var deleteOrderRequestHandler = CreateDeleteOrderRequestHandler();

            var request = new DeleteOrderRequest() { OrderId = -1};

            var command = new DeleteOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await deleteOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsNull_ShouldReturnBadRequestResult()
        {
            // Arrange
            var createOrderRequestHandler = CreateDeleteOrderRequestHandler();

            var command = new DeleteOrderCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await createOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
