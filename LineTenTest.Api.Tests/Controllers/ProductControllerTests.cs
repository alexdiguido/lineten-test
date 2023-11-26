using LineTenTest.Api.Controllers;
using MediatR;
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
using Microsoft.AspNetCore.Mvc;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Api.Tests.Controllers
{
    public class ProductControllerTests
    {
        private AutoMocker _mockRepository = new();

        private const string ExceptionMessage = $"an error occurred. Please contact Application admin";

        private ProductController CreateProductController()
        {
            return _mockRepository.CreateInstance<ProductController>();
        }

        [Fact]
        public async Task Get_OrderIdValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int productId = 1;
            var productController = CreateProductController();
            var expectedDto = new ProductDto()
            {
                Id = productId
            };

            _mockRepository.GetMock<IMediator>()
                .Setup(expression: m => m.Send(It.Is<GetProductByIdQuery>(q=> q.ProductId == productId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(expectedDto));
            // Act
            var result = await productController.Get(productId);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Get_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int productId = 1;
            var productController = CreateProductController();

            var expectedStatusCode = 500;
            var expectedMessage = ExceptionMessage;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.Is<GetProductByIdQuery>(q=> q.ProductId == productId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await productController.Get(productId);

            var objectResult = result.Result as ObjectResult;
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

         [Fact]
        public async Task Create_OrderIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int orderId = 1;
            var productController = CreateProductController();
            var productDto = ArrangeProductDto();
            var createProductRequest = new CreateProductRequest()
            {
                Sku = "sku",
                Name = "name",
                Description = "description"
            };

            var expectedStatusCode = 200;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => 
                m.Send(It.Is<CreateProductCommand>(q=> 
                    q.Request.Name == createProductRequest.Name && 
                    q.Request.Sku == createProductRequest.Sku &&
                    q.Request.Description == createProductRequest.Description), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(productDto));
            // Act
            var result = await productController.Create(createProductRequest);

            var objectResult = result.Result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(productDto);
            _mockRepository.VerifyAll();
        }

        

        [Fact]
        public async Task Create_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            var productController = CreateProductController();
            var createProductRequest = new CreateProductRequest()
            {
                Sku = "sku",
                Name = "name",
                Description = "description"
            };
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.IsAny<CreateOrderCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await productController.Create(createProductRequest);

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();

            var objectResult = result.Result as ObjectResult;
            
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

        private static ProductDto ArrangeProductDto()
        {
            return new ProductDto()
            {
                Id = 1,
                SKU = "sku",
                Description = "description",
                Name = "name"
            };
        }
    }
}
