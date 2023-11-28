using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LineTenTest.Api.IntegrationTests
{
    public class OrderControllersIntegrationTests : IClassFixture<IntTestWebApplicationFactory<Program>>
    {
        private readonly IntTestWebApplicationFactory<Program> _factory;

        public OrderControllersIntegrationTests(IntTestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OrderController_Get_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var orderId = 1;

            // Act
            var response = await client.GetAsync($"/api/v1/order/get?orderId={orderId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            Assert.NotNull(order);
        }

        [Fact]
        public async Task OrderController_Create_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var createOrderRequest = new CreateOrderRequest
            {
                ProductId = 1,
                CustomerId = 1,
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/order/create", createOrderRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
            Assert.NotNull(createdOrder);
        }

        [Fact]
        public async Task OrderController_Update_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var createOrderRequest = new UpdateOrderRequest()
            {
                OrderId = 1,
                CustomerId = 1,
                ProductId = 1,
                Status = 5
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/order/update", createOrderRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
            Assert.NotNull(createdOrder);
        }

        [Fact]
        public async Task OrderController_Delete_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var deleteOrderRequest = new DeleteOrderRequest()
            {
                OrderId = 1,
            };

            // Act
            var response = await client.DeleteAsync($"/api/v1/order/delete?OrderId={deleteOrderRequest.OrderId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var createdOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
            Assert.NotNull(createdOrder);
        }
    }
}
