using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Commands;
using Moq.AutoMock;
using Xunit;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Services.Order;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Order;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Tests.Services
{
    public class CreateOrderRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();


        private CreateOrderRequestHandler CreateCreateOrderRequestHandler()
        {
            return _mockRepository.CreateInstance<CreateOrderRequestHandler>();
        }

       [Fact] 
       public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var createOrderRequestHandler = CreateCreateOrderRequestHandler();

            var request = new CreateOrderRequest()
            {
                CustomerId = 1,
                ProductId = 1
            };

            var command = new CreateOrderCommand(request);
            CancellationToken cancellationToken = default;
            Order orderEntity = OrderBuilder.CreateDefault();

            _mockRepository.GetMock<ICreateOrderService>().Setup(s => s.CreateAsync(request))
                .ReturnsAsync(orderEntity);

            // Act
            var result = await createOrderRequestHandler.Handle(command, cancellationToken);

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
            var createOrderRequestHandler = CreateCreateOrderRequestHandler();
            var request = new CreateOrderRequest()
            {
                CustomerId = 1,
                ProductId = 1
            };

            var command = new CreateOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<ICreateOrderService>().Setup(s => s.CreateAsync(It.IsAny<CreateOrderRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await createOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);
            objectResult.Value.Should().Be(Constants.InternalServerErrorResultMessage);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(-1,-1)]
        [InlineData(1,-1)]
        [InlineData(-1,1)]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(int customerId, int productId)
        {
            // Arrange
            var createOrderRequestHandler = CreateCreateOrderRequestHandler();
            var request = new CreateOrderRequest()
            {
                CustomerId = customerId,
                ProductId = productId
            };

            var command = new CreateOrderCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await createOrderRequestHandler.Handle(command, cancellationToken);

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
            var createOrderRequestHandler = CreateCreateOrderRequestHandler();

            var command = new CreateOrderCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await createOrderRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
