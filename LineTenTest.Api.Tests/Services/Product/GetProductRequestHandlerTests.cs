using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Services.Product;
using LineTenTest.Domain.Services.Product;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Utilities;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Product
{
    public class GetProductRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private GetProductRequestHandler CreateCreateProductRequestHandler()
        {
            return _mockRepository.CreateInstance<GetProductRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var productRequestHandler = CreateCreateProductRequestHandler();
            var productId = 1;
            var command = new GetProductByIdQuery(productId);
            CancellationToken cancellationToken = default;
            var productEntity = ProductBuilder.CreateDefault(); 
            _mockRepository.GetMock<IGetProductService>().Setup(s => s.GetAsync(productId))
                .ReturnsAsync(productEntity);

            // Act
            var result = await productRequestHandler.Handle(command, cancellationToken);

            // Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
            var value = (ProductDto)objectResult.Value!;
            value.ShouldBeEquivalentTo(productEntity);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceThrowException_ShouldReturnInternalServerErrorResult()
        {
            // Arrange
            var productRequestHandler = CreateCreateProductRequestHandler();
            var productId = 1;
            var command = new GetProductByIdQuery(productId);
            CancellationToken cancellationToken = default;
            var productEntity = ProductBuilder.CreateDefault(); 
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IGetProductService>().Setup(s => s.GetAsync(productId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await productRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
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
            var productRequestHandler = CreateCreateProductRequestHandler();
            var productId = -1;
            var command = new GetProductByIdQuery(productId);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await productRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
