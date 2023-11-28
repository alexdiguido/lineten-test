using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Domain.Services.Customer;
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

namespace LineTenTest.Api.Tests.Services.Customer
{
    public class UpdateCustomerRequestHandlerTests
    {
         private AutoMocker _mockRepository = new();

        private UpdateCustomerRequestHandler CreateUpdateCustomerRequestHandler()
        {
            return _mockRepository.CreateInstance<UpdateCustomerRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var updateCustomerRequestHandler = CreateUpdateCustomerRequestHandler();

            var request = new UpdateCustomerRequest()
            {
                CustomerId = 1,
                Email = "email@gmail.com",
                FirstName = "firstName",
                LastName = "lastName",
                Phone = "phone"
            };

            var command = new UpdateCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var customerEntity = CustomerBuilder.CreateDefault(); 
            _mockRepository.GetMock<IUpdateCustomerService>().Setup(s => s.UpdateAsync(request))
                .ReturnsAsync(customerEntity);

            // Act
            var result = await updateCustomerRequestHandler.Handle(command, cancellationToken);

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
            var updateCustomerRequestHandler = CreateUpdateCustomerRequestHandler();

            var request = new UpdateCustomerRequest()
            {
                CustomerId = 1,
                Email = "email@gmail.com",
                FirstName = "firstName",
                LastName = "lastName",
                Phone = "phone"
            };

            var command = new UpdateCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IUpdateCustomerService>().Setup(s => s.UpdateAsync(It.IsAny<UpdateCustomerRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await updateCustomerRequestHandler.Handle(command, cancellationToken);

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
        [InlineData(1,"firstName", "lastName","phone", "")]
        [InlineData(1,"firstName", "lastName","phone", null)]
        [InlineData(1,"firstName", "lastName","", "email")]
        [InlineData(1,"firstName", "lastName",null, "email")]
        [InlineData(1,"firstName", "","phone", "email")]
        [InlineData(1,"firstName", null,"phone", "email")]
        [InlineData(1,"", "lastname","phone", "email")]
        [InlineData(1,null, "lastname","phone", "email")]
        [InlineData(1,"firstname", "lastname","phone", "email")]
        public async Task Handle_RequestIsNotValid_ShouldReturnBadRequestResult(int id, string firstName, string lastName, string phone, string email)
        {
            // Arrange
            var updateCustomerRequestHandler = CreateUpdateCustomerRequestHandler();

            var request = new UpdateCustomerRequest()
            {
                CustomerId = id,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email
            };

            var command = new UpdateCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await updateCustomerRequestHandler.Handle(command, cancellationToken);

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
            var customerRequestHandler = CreateUpdateCustomerRequestHandler();

            var command = new UpdateCustomerCommand(null);
            CancellationToken cancellationToken = default;
            var expectedStatus = 400;

            // Act
            var result = await customerRequestHandler.Handle(command, cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var objectResult = result.Result as BadRequestObjectResult;

            objectResult.StatusCode.Should().Be(expectedStatus);

            _mockRepository.VerifyAll();
        }
    }
}
