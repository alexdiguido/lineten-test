using System.Net.Http.Json;
using System.Text;
using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Entities;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel.ApiModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LineTenTest.Api.IntegrationTests
{
    public class CustomerControllersIntegrationTests : IClassFixture<IntTestWebApplicationFactory<Program>>
    {
        private readonly IntTestWebApplicationFactory<Program> _factory;

        public CustomerControllersIntegrationTests(IntTestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CustomerController_Get_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var customerId = 1;

            // Act
            var response = await client.GetAsync($"/api/v1/customer/get?customerId={customerId}");

            // Assert
            response.EnsureSuccessStatusCode(); 
            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            Assert.NotNull(customer);
        }

        [Fact]
        public async Task CustomerController_Create_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var createCustomerRequest = new CreateCustomerRequest
            {
                Email = "email@gmail.com",
                Phone = "phone",
                FirstName = "firstName",
                LastName = "lastname"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/customer/create", createCustomerRequest);

            // Assert
            response.EnsureSuccessStatusCode(); 
            var createdCustomer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            Assert.NotNull(createdCustomer);
        }

        [Fact]
        public async Task CustomerController_Update_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var updateCustomerRequest = new UpdateCustomerRequest()
            {
                CustomerId = 2,
                Email = "email@gmail.com",
                Phone = "phone",
                FirstName = "firstName",
                LastName = "lastname"
            };

            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(updateCustomerRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await client.PutAsync("/api/v1/customer/update", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdCustomer = await response.Content.ReadFromJsonAsync<CustomerDto>();
            Assert.NotNull(createdCustomer);
        }

        [Fact]
        public async Task CustomerController_Delete_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var deleteCustomerRequest = new DeleteCustomerRequest()
            {
                CustomerId = 5,
            };

            // Act
            var response = await client.DeleteAsync($"/api/v1/customer/delete?customerId={deleteCustomerRequest.CustomerId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}