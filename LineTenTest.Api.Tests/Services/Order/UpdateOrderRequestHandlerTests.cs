using LineTenTest.Api.Services.Order;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Services.Order;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Tests.Services.Order
{
    public class UpdateOrderRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private UpdateOrderRequestHandler CreateUpdateOrderRequestHandler()
        {
            return _mockRepository.CreateInstance<UpdateOrderRequestHandler>();
        }

       
       [Fact] 
       public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var requestHandler = CreateUpdateOrderRequestHandler();

            var request = new UpdateOrderRequest()
            {
                OrderId = 1,
                Status = 3,
                CustomerId = 1,
                ProductId = 1
            };

            var command = new UpdateOrderCommand(request);
            CancellationToken cancellationToken = default;
            Domain.Entities.Order orderEntity = OrderBuilder.CreateDefault();

            _mockRepository.GetMock<IUpdateOrderService>().Setup(s => s.UpdateAsync(request))
                .ReturnsAsync(orderEntity);

            // Act
            var result = await requestHandler.Handle(command, cancellationToken);

            // Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
            var value = (OrderDto)objectResult.Value!;
            value.ShouldBeEquivalentTo(orderEntity);

            _mockRepository.VerifyAll();
        }

        [Fact] public async Task Handle_RequestIsValid_DomainServiceThrowException_ShouldReturnInternalServerErrorResult()
        {
            // Arrange
            var updateOrderRequestHandler = CreateUpdateOrderRequestHandler();
            var request = new UpdateOrderRequest()
            {
                CustomerId = 1,
                ProductId = 1,
                Status = 3,
                OrderId = 1,
            };

            var command = new UpdateOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IUpdateOrderService>().Setup(s => s.UpdateAsync(It.IsAny<UpdateOrderRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await updateOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);
            objectResult.Value.Should().Be(Constants.InternalServerErrorResultMessage);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(-1,-1,1,1)]
        [InlineData(1,-1,1,1)]
        [InlineData(-1,1,1,1)]
        [InlineData(1,1,100,1)]
        [InlineData(1,1,1, -1)]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(int customerId, int productId, int status, int orderId)
        {
            // Arrange
            var requestHandler = CreateUpdateOrderRequestHandler();
            var request = new UpdateOrderRequest()
            {
                CustomerId = customerId,
                ProductId = productId,
                Status = status,
                OrderId = orderId
            };

            var command = new UpdateOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await requestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsNull_ShouldReturnBadRequestResult()
        {
            // Arrange
            var requestHandler = CreateUpdateOrderRequestHandler();

            var command = new UpdateOrderCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await requestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
