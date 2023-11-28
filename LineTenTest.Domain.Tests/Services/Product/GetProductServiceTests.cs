using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Product;
using LineTenTest.Domain.Services.Product;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Product
{
    public class GetProductServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly EfRepository<Domain.Entities.Product> _productRepository;

        public GetProductServiceTests(DatabaseFixture dbFixture)
        {
            _productRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private GetProductService CreateService()
        {
            return new GetProductService(_productRepository);
        }

        [Fact] 
        public async Task GetAsync_ProductExist_ShouldReturnProductAllProductData()
        {
            // Arrange
            var service = CreateService();
            int productId = 1;

            // Act
            var result = await service.GetAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Should().NotBeNull();
            result.Description.Should().NotBeNullOrWhiteSpace();
            result.SKU.Should().NotBeNullOrWhiteSpace();
            result.Name.Should().NotBeNullOrWhiteSpace();
        }

        [Fact] 
        public async Task GetAsync_ProductNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var service = CreateService();
            int productId = 100;
            string message = "Product not found";

            // Act
            var result = async () => await service.GetAsync(productId);

            // Assert

            await result.Should().ThrowAsync<NotFoundException>(message);
        }
    }
}
