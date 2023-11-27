using FluentAssertions;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LineTenTest.Api.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private AutoMocker _mockRepository = new();

        private const string ExceptionMessage = $"an error occurred. Please contact Application admin";

        private CustomerController CreateCustomerController()
        {
            return _mockRepository.CreateInstance<CustomerController>();
        }

        [Fact]
        public async Task Get_CustomerIdValid_ShouldReturnOkActionResult()
        {
            // Arrange
            int customerId = 1;
            var customerController = CreateCustomerController();
            var expectedDto = new CustomerDto()
            {
                Id = customerId
            };

            _mockRepository.GetMock<IMediator>()
                .Setup(expression: m => m.Send(It.Is<GetCustomerByIdQuery>(q=> q.CustomerId == customerId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(expectedDto));
            // Act
            var result = await customerController.Get(customerId);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Get_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            int customerId = 1;
            var customerController = CreateCustomerController();

            var expectedStatusCode = 500;
            var expectedMessage = ExceptionMessage;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.Is<GetCustomerByIdQuery>(q=> q.CustomerId == customerId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await customerController.Get(customerId);

            var objectResult = result.Result as ObjectResult;
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

         [Fact]
        public async Task Create_RequestIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var customerDto = ArrangeCustomerDto();
            var createCustomerRequest = new CreateCustomerRequest()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };

            var expectedStatusCode = 200;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => 
                m.Send(It.Is<CreateCustomerCommand>(q=> 
                    q.Request.FirstName == createCustomerRequest.FirstName && 
                    q.Request.LastName == createCustomerRequest.LastName &&
                    q.Request.Email == createCustomerRequest.Email &&
                    q.Request.Phone == createCustomerRequest.Phone), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(customerDto));
            // Act
            var result = await customerController.Create(createCustomerRequest);

            var objectResult = result.Result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(customerDto);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var customerDto = ArrangeCustomerDto();
            var createCustomerRequest = new CreateCustomerRequest()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.IsAny<CreateCustomerCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await customerController.Create(createCustomerRequest);

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();

            var objectResult = result.Result as ObjectResult;
            
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update_RequestIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var customerDto = ArrangeCustomerDto();
            var updateCustomerRequest = new UpdateCustomerRequest()
            {
                CustomerId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };

            var expectedStatusCode = 200;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => 
                m.Send(It.Is<UpdateCustomerCommand>(q=> q.Request.CustomerId == updateCustomerRequest.CustomerId &&
                                                       q.Request.FirstName == updateCustomerRequest.FirstName && 
                                                       q.Request.LastName == updateCustomerRequest.LastName &&
                                                       q.Request.Email == updateCustomerRequest.Email &&
                                                       q.Request.Phone == updateCustomerRequest.Phone), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(customerDto));
            // Act
            var result = await customerController.Update(updateCustomerRequest);

            var objectResult = result.Result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(customerDto);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Update_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var updateCustomerRequest = new UpdateCustomerRequest()
            {
                CustomerId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.IsAny<UpdateCustomerCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await customerController.Update(updateCustomerRequest);

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();

            var objectResult = result.Result as ObjectResult;
            
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

         [Fact]
        public async Task Delete_RequestIsValid_ShouldReturnOkActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var customerDto = ArrangeCustomerDto();
            var deleteCustomerRequest = new DeleteCustomerRequest()
            {
                CustomerId = 1,
            };

            var expectedStatusCode = 200;

            _mockRepository.GetMock<IMediator>().Setup(expression: m => 
                m.Send(It.Is<DeleteCustomerCommand>(q=> 
                        q.Request.CustomerId == deleteCustomerRequest.CustomerId), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OkObjectResult(customerDto));
            // Act
            var result = await customerController.Delete(deleteCustomerRequest);

            var objectResult = result as OkObjectResult;

            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(customerDto);
            _mockRepository.VerifyAll();
        }

        

        [Fact]
        public async Task Delete_MediatorThrowsException_ShouldReturnInternalServerErrorActionResult()
        {
            // Arrange
            var customerController = CreateCustomerController();
            var deleteCustomerRequest = new DeleteCustomerRequest() { };
            var expectedStatusCode = 500;
            var expectedMessage = $"an error occurred. Please contact Application admin";

            _mockRepository.GetMock<IMediator>().Setup(expression: m => m.Send(It.IsAny<DeleteCustomerCommand>(),It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("exception message with internal stacktrace"));
            // Act
            var result = await customerController.Delete(deleteCustomerRequest);

            result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();

            var objectResult = result as ObjectResult;
            
            // Assert
            objectResult.StatusCode.Should().Be(expectedStatusCode);
            objectResult.Value.Should().Be(expectedMessage);
            _mockRepository.VerifyAll();
        }

        private static CustomerDto ArrangeCustomerDto()
        {
            return new CustomerDto()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = "Phone",
                Email = "Email"
            };
        }
    }
}
