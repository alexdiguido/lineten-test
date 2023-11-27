using LineTenTest.Api.Services.Product;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Commands;
using Moq.AutoMock;
using Xunit;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Services.Order;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using LineTenTest.Domain.Services.Product;

namespace LineTenTest.Api.Tests.Services.Product
{
    public class CreateProductRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private CreateProductRequestHandler CreateCreateProductRequestHandler()
        {
            return _mockRepository.CreateInstance<CreateProductRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var createProductRequestHandler = CreateCreateProductRequestHandler();

            var request = new CreateProductRequest()
            {
                Name = "name",
                Description = "description",
                Sku = "sku"
            };

            var command = new CreateProductCommand(request);
            CancellationToken cancellationToken = default;
            var productEntity = ProductBuilder.CreateDefault(); 
            _mockRepository.GetMock<ICreateProductService>().Setup(s => s.CreateAsync(request))
                .ReturnsAsync(productEntity);

            // Act
            var result = await createProductRequestHandler.Handle(command, cancellationToken);

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
            var request = new CreateProductRequest()
            {
                Description = "description",
                Name = "name",
                Sku = "Sku"
            };

            var command = new CreateProductCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<ICreateProductService>().Setup(s => s.CreateAsync(It.IsAny<CreateProductRequest>()))
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

        [Theory]
        [InlineData("", "sku","description")]
        [InlineData("name","","description")]
        [InlineData("name","sku","" )]
        [InlineData(null,"sku","description" )]
        [InlineData("name",null,"description" )]
        [InlineData("name","sku",null )]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(string name, string sku, string description)
        {
            // Arrange
            var productRequestHandler = CreateCreateProductRequestHandler();
            var request = new CreateProductRequest()
            {
                Description = description,
                Name = name,
                Sku = sku
            };

            var command = new CreateProductCommand(request);
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

        [Fact]
        public async Task Handle_RequestIsNull_ShouldReturnBadRequestResult()
        {
            // Arrange
            var productRequestHandler = CreateCreateProductRequestHandler();

            var command = new CreateProductCommand(null);
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
