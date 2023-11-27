using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Services.Product;
using LineTenTest.Domain.Services.Product;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;
using FluentAssertions;
using LineTenTest.Api.Utilities;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Product
{
    public class UpdateProductRequestHandlerTests
    {
       private AutoMocker _mockRepository = new();

        private UpdateProductRequestHandler CreateUpdateProductRequestHandler()
        {
            return _mockRepository.CreateInstance<UpdateProductRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var updateProductRequestHandler = CreateUpdateProductRequestHandler();

            var request = new UpdateProductRequest()
            {
                ProductId = 1,
                Name = "name",
                Description = "description",
                Sku = "sku"
            };

            var command = new UpdateProductCommand(request);
            CancellationToken cancellationToken = default;
            var productEntity = ProductBuilder.CreateDefault(); 
            _mockRepository.GetMock<IUpdateProductService>().Setup(s => s.UpdateAsync(request))
                .ReturnsAsync(productEntity);

            // Act
            var result = await updateProductRequestHandler.Handle(command, cancellationToken);

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
            var updateProductRequestHandler = CreateUpdateProductRequestHandler();

            var request = new UpdateProductRequest()
            {
                ProductId = 1,
                Name = "name",
                Description = "description",
                Sku = "sku"
            };

            var command = new UpdateProductCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IUpdateProductService>().Setup(s => s.UpdateAsync(It.IsAny<UpdateProductRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await updateProductRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);
            objectResult.Value.Should().Be(Constants.InternalServerErrorResultMessage);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(1,"", "sku","description")]
        [InlineData(1,"name","","description")]
        [InlineData(1,"name","sku","" )]
        [InlineData(1,null,"sku","description" )]
        [InlineData(1,"name",null,"description" )]
        [InlineData(1,"name","sku",null )]
        [InlineData(-1,"name","sku","description" )]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(int id, string name, string sku, string description)
        {
            // Arrange
            var updateProductRequestHandler = CreateUpdateProductRequestHandler();

            var request = new UpdateProductRequest()
            {
                ProductId = id,
                Name = name,
                Description = description,
                Sku = sku
            };

            var command = new UpdateProductCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await updateProductRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsNull_ShouldReturnBadRequestResult()
        {
            // Arrange
            var productRequestHandler = CreateUpdateProductRequestHandler();

            var command = new UpdateProductCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await productRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
