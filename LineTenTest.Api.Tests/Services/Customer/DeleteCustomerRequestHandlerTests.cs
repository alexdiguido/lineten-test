using LineTenTest.Api.Commands;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Utilities;
using LineTenTest.SharedKernel.ApiModels;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Customer
{
    public class DeleteCustomerRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private DeleteCustomerRequestHandler CreateDeleteCustomerRequestHandler()
        {
            return _mockRepository.CreateInstance<DeleteCustomerRequestHandler>();
        }

        [Fact]
        public async Task Handle_RequestIsValid_DomainServiceReturnOk_ShouldReturnOkResult()
        {
            // Arrange
            var deleteOrderRequestHandler = CreateDeleteCustomerRequestHandler();

            var request = new DeleteCustomerRequest() { CustomerId = 1};

            var command = new DeleteCustomerCommand(request);
            CancellationToken cancellationToken = default;
            Domain.Entities.Customer customerEntity = CustomerBuilder.CreateDefault();

            _mockRepository.GetMock<IDeleteCustomerService>().Setup(s => s.DeleteAsync(request))
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
            var deleteOrderRequestHandler = CreateDeleteCustomerRequestHandler();

            var request = new DeleteCustomerRequest() { CustomerId = 1};

            var command = new DeleteCustomerCommand(request);
            CancellationToken cancellationToken = default;
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IDeleteCustomerService>().Setup(s => s.DeleteAsync(It.IsAny<DeleteCustomerRequest>()))
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
            var deleteOrderRequestHandler = CreateDeleteCustomerRequestHandler();

            var request = new DeleteCustomerRequest() { CustomerId = -1};

            var command = new DeleteCustomerCommand(request);
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
