using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Api.Services.Customer;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Utilities;
using Xunit;

namespace LineTenTest.Api.Tests.Services.Customer
{
    public class GetCustomerRequestHandlerTests
    {
        private AutoMocker _mockRepository = new();

        private GetCustomerRequestHandler CreateCreateCustomerRequestHandler()
        {
            return _mockRepository.CreateInstance<GetCustomerRequestHandler>();
        }

        [Fact]
        public async Task
            Handle_RequestIsValid_DomainServiceReturnOrderEntity_ShouldReturnOkObjectResultWithOrderDataDto()
        {
            // Arrange
            var customerRequestHandler = CreateCreateCustomerRequestHandler();
            var customerId = 1;
            var command = new GetCustomerByIdQuery(customerId);
            CancellationToken cancellationToken = default;
            var customerEntity = CustomerBuilder.CreateDefault();
            _mockRepository.GetMock<IGetCustomerService>().Setup(s => s.GetAsync(customerId))
                .ReturnsAsync(customerEntity);

            // Act
            var result = await customerRequestHandler.Handle(command, cancellationToken);

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
            var customerRequestHandler = CreateCreateCustomerRequestHandler();
            var customerId = 1;
            var command = new GetCustomerByIdQuery(customerId);
            CancellationToken cancellationToken = default;
            var customerEntity = CustomerBuilder.CreateDefault();
            var expectedStatus = 500;
            var exceptionMessage = "message";
            _mockRepository.GetMock<IGetCustomerService>().Setup(s => s.GetAsync(customerId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await customerRequestHandler.Handle(command, cancellationToken);

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
            var customerRequestHandler = CreateCreateCustomerRequestHandler();
            var customerId = -1;
            var command = new GetCustomerByIdQuery(customerId);
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
    }
}
