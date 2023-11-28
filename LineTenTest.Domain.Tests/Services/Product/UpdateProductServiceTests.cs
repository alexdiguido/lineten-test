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
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Product
{
    public class UpdateProductServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Product> _productRepository;

        public UpdateProductServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _productRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private UpdateProductService CreateService()
        {
            return new UpdateProductService(_productRepository);
        }

        [Fact] 
        public async Task UpdateAsync_ProductExists_ShouldReturnOrderAllOrderData()
        {
            // Arrange
            var service = CreateService();
            var nameupdated = "nameupdated";
            var descupdated = "descupdated";
            var skuupdated = "skuupdated";
            var product = new UpdateProductRequest()
            {
                ProductId = 1,
                Name = nameupdated,
                Description = descupdated,
                Sku = skuupdated
            };

            // Act
            var result = await service.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be(descupdated);
            result.Name.Should().Be(nameupdated);
            result.SKU.Should().Be(skuupdated);
            _dbFixture.DbContext.Products.Should().Contain(product => product.Name == nameupdated &&
                                                                        product.Description == descupdated &&
                                                                        product.SKU == skuupdated);
        }

        [Fact] 
        public async Task UpdateAsync_ProductNotExists_ShouldReturnNotFoundException()
        {
            // Arrange
            var service = CreateService();
            var product = new UpdateProductRequest()
            {
                ProductId = 223 // id should not exist in the dbContext
            };

            // Act
            var result = async () => await service.UpdateAsync(product);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}
