using FluentAssertions;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Product;
using LineTenTest.Domain.Services.Product;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;
using Moq;
using System;
using System.Threading.Tasks;
using LineTenTest.Api.Commands;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Product
{
    public class DeleteProductServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Product> _productRepository;

        public DeleteProductServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _productRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private DeleteProductService CreateService()
        {
            return new DeleteProductService(_productRepository);
        }

        [Fact] 
        public async Task DeleteAsync_ProductExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var product = new DeleteProductRequest()
            {
                ProductId = 1
            };

            // Act
            await service.DeleteAsync(product);
            _dbFixture.DbContext.Products.Should().NotContain(c => c.Id == product.ProductId);
        }

        [Fact] 
        public async Task DeleteAsync_ProductNotExists_ShouldReturnOrderAllOrderData()
        {
            var service = CreateService();
            int productId = 100;
            string message = "Product not found";

            // Act
            var result = async () => await service.DeleteAsync(new DeleteProductRequest() {ProductId = productId});

            // Assert

            await result.Should().ThrowAsync<EntityNotFoundException>(message);
        }
    }
}
