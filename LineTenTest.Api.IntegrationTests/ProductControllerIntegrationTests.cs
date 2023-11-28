using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.Api.Commands;

namespace LineTenTest.Api.IntegrationTests
{
    public class ProductControllersIntegrationTests : IClassFixture<IntTestWebApplicationFactory<Program>>
    {
        private readonly IntTestWebApplicationFactory<Program> _factory;

        public ProductControllersIntegrationTests(IntTestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ProductController_Get_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var productId = 1;

            // Act
            var response = await client.GetAsync($"/api/v1/product/get?productId={productId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
        }

        [Fact]
        public async Task ProductController_Create_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var createProductRequest = new CreateProductRequest
            {
                Name = "name",
                Description = "description",
                Sku = "sku"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/product/create", createProductRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(createdProduct);
        }

        [Fact]
        public async Task ProductController_Update_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var createProductRequest = new UpdateProductRequest()
            {
                ProductId = 1,
                Name = "name",
                Description = "description",
                Sku = "sku"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/product/update", createProductRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(createdProduct);
        }

        [Fact]
        public async Task ProductController_Delete_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var deleteProductRequest = new DeleteProductRequest()
            {
                ProductId = 1,
            };

            // Act
            var response =
                await client.DeleteAsync($"/api/v1/product/delete?ProductId={deleteProductRequest.ProductId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(createdProduct);
        }
    }
}
