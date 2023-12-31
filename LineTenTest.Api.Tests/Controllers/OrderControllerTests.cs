﻿using LineTenTest.Api.Controllers;
using System;
using FluentAssertions;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LineTenTest.Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMediator> _mockMediator;
        private const string ExceptionMessage = $"an error occurred. Please contact Application admin";

        public OrderControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();
        }

        public OrderController CreateService()
        {
            return new OrderController(_mockMediator.Object);
        }

        [Fact]
        public async Task Get_OrderIdValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var expectedDto = new OrderDto
            {
                OrderId = orderId
            };

            _mockMediator.Setup(expression: m => m.Send(It.Is<GetOrderByIdQuery>(q=> q.OrderId == orderId), It.IsAny<CancellationToken>())).ReturnsAsync(new OkObjectResult(expectedDto));
            // Act
            var result = await orderController.Get(orderId);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Get_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();

            var expectedStatusCode = 500;
            var expectedMessage = ExceptionMessage;

            _mockMediator.Setup(expression: m => m.Send(It.Is<GetOrderByIdQuery>(q=> q.OrderId == orderId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await orderController.Get(orderId);

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
            var orderController = CreateService();
            var createOrderDto = new CreateOrderRequest
            {
                CustomerId = 11,
                ProductId = 22
            };
            var expectedStatusCode = 200;

            var orderDto = ArrangeOrderDto();

            _mockMediator.Setup(expression: m => 
                m.Send(It.Is<CreateOrderCommand>(q=> 
                    q.CreateOrderRequest.ProductId == createOrderDto.ProductId && 
                    q.CreateOrderRequest.CustomerId == createOrderDto.CustomerId), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(orderDto));
            // Act
            var result = await orderController.Create(createOrderDto);

            var objectResult = result.Result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(orderDto);
            _mockRepository.VerifyAll();
        }

        private static OrderDto ArrangeOrderDto()
        {
            return new OrderDto()
            {
                OrderId = 1,
                Status = 1,
                CreatedDate = new DateTime(2023, 11, 26, 11, 12, 13),
                UpdateDate = new DateTime(2023, 11, 26, 11, 12, 13),
                Product = new ProductDto()
                {
                    Id = 1,
                    SKU = "sku",
                    Description = "description",
                    Name = "name"
                },
                Customer = new CustomerDto()
                {
                    Email = "email",
                    FirstName = "firstName",
                    Id = 2,
                    LastName = "lastName",
                    Phone = "phone"
                }
            };
        }

        [Fact]
        public async Task Create_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var createOrderDto = new CreateOrderRequest
            {
                CustomerId = 11,
                ProductId = 22
            };
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockMediator.Setup(expression: m => m.Send(It.IsAny<CreateOrderCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await orderController.Create(createOrderDto);

            var objectResult = result.Result as ObjectResult;
            // Assert
            
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update_OrderIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var updateOrderRequest = new UpdateOrderRequest
            {
                OrderId = orderId,
                CustomerId = 11,
                ProductId = 22,
                Status = 2
            };
            var expectedStatusCode = 200;

            var orderDto = ArrangeOrderDto();

            _mockMediator.Setup(expression: m => 
                    m.Send(It.Is<UpdateOrderCommand>(q=> 
                            q.Request.ProductId == updateOrderRequest.ProductId && 
                            q.Request.CustomerId == updateOrderRequest.CustomerId &&
                            q.Request.Status == updateOrderRequest.Status), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(orderDto));
            // Act
            var result = await orderController.Update(updateOrderRequest);

            var objectResult = result.Result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(orderDto);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var updateOrderRequest = new UpdateOrderRequest();
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockMediator.Setup(expression: m => m.Send(It.IsAny<UpdateOrderCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await orderController.Update(updateOrderRequest);

            var objectResult = result.Result as ObjectResult;
            // Assert
            
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_OrderIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var deleteOrderRequest = new DeleteOrderRequest
            {
                OrderId = orderId,
            };
            var expectedStatusCode = 200;

            var orderDto = ArrangeOrderDto();

            _mockMediator.Setup(expression: m => 
                    m.Send(It.Is<DeleteOrderCommand>(q=> 
                            q.Request.OrderId == orderId), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(orderDto));
            // Act
            var result = await orderController.Delete(deleteOrderRequest);

            var objectResult = result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(orderDto);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int orderId = 1;
            var orderController = CreateService();
            var updateOrderRequest = new DeleteOrderRequest();
            var expectedStatusCode = 500;
            var expectedMessage = ExceptionMessage;

            _mockMediator.Setup(expression: m => m.Send(It.IsAny<DeleteOrderCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await orderController.Delete(updateOrderRequest);

            var objectResult = result as ObjectResult;
            // Assert
            
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }
    }
}
