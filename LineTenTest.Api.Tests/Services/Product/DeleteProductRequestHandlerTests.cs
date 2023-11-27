using LineTenTest.Api.Commands;
using LineTenTest.Api.Services.Order;
using LineTenTest.Api.Services.Product;
using LineTenTest.Domain.Services.Order;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Services.Product;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Product
{
    public class DeleteProductRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private DeleteProductRequestHandler CreateDeleteProductRequestHandler()
        {
            return _mockRepository.CreateInstance<DeleteProductRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOk_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var deleteOrderRequestHandler = CreateDeleteProductRequestHandler();

            var request = new DeleteProductRequest() { ProductId = 1};

            var command = new DeleteProductCommand(request);
            CancellationToken cancellationToken = default;
            Domain.Entities.Product orderEntity = ProductBuilder.CreateDefault();

            _mockRepository.GetMock<IDeleteProductService>().Setup(s => s.DeleteAsync(request))
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
            var deleteOrderRequestHandler = CreateDeleteProductRequestHandler();

            var request = new DeleteProductRequest() { ProductId = 1};

            var command = new DeleteProductCommand(request);
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
            var deleteOrderRequestHandler = CreateDeleteProductRequestHandler();

            var request = new DeleteProductRequest() { ProductId = -1};

            var command = new DeleteProductCommand(request);
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
    }
}
