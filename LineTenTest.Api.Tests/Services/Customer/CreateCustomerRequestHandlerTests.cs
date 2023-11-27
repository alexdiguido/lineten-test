using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.SharedKernel.ApiModels;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System;
using FluentAssertions;
using LineTenTest.Api.Controllers;
using LineTenTest.Api.Utilities;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Customer
{
    public class CreateCustomerRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private CreateCustomerRequestHandler CreateCustomerRequestHandler()
        {
            return _mockRepository.CreateInstance<CreateCustomerRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var createCustomerRequestHandler = CreateCustomerRequestHandler();

            var createCustomerRequest = new CreateCustomerRequest()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };

            var command = new CreateCustomerCommand(createCustomerRequest);
            CancellationToken cancellationToken = default;
            var customerEntity = CustomerBuilder.CreateDefault(); 
            _mockRepository.GetMock<ICreateCustomerService>().Setup(s => s.CreateAsync(createCustomerRequest))
                .ReturnsAsync(customerEntity);

            // Act
            var result = await createCustomerRequestHandler.Handle(command, cancellationToken);

            // Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
            var value = (CustomerDto)objectResult.Value!;
            value.ShouldBeEquivalentTo(customerEntity);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceThrowException_ShouldReturnInternalServerErrorResult()
        {
            // Arrange
            var createCustomerRequestHandler = CreateCustomerRequestHandler();
            var request = new CreateCustomerRequest()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Phone = "Phone"
            };

            var command = new CreateCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<ICreateCustomerService>().Setup(s => s.CreateAsync(It.IsAny<CreateCustomerRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await createCustomerRequestHandler.Handle(command, cancellationToken);

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
        [InlineData("firstName", "lastName","phone", "")]
        [InlineData("firstName", "lastName","phone", null)]
        [InlineData("firstName", "lastName","", "email")]
        [InlineData("firstName", "lastName",null, "email")]
        [InlineData("firstName", "","phone", "email")]
        [InlineData("firstName", null,"phone", "email")]
        [InlineData("", "lastname","phone", "email")]
        [InlineData(null, "lastname","phone", "email")]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(string firstName, string lastName, string phone, string email)
        {
            // Arrange
            var customerRequestHandler = CreateCustomerRequestHandler();
            var request = new CreateCustomerRequest()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            };

            var command = new CreateCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await customerRequestHandler.Handle(command, cancellationToken);

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
            var createCustomerRequestHandler = CreateCustomerRequestHandler();

            var command = new CreateCustomerCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await createCustomerRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
