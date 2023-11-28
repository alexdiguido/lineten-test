using LineTenTest.Domain.Services.Product;
using LineTenTest.SharedKernel;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel.ApiModels;
using Xunit;

namespace LineTenTest.Domain.Tests.Services.Product
{
    public class CreateProductServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly EfRepository<Domain.Entities.Product> _orderRepository;

        public CreateProductServiceTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _orderRepository = new EfRepository<Domain.Entities.Product>(dbFixture.DbContext);
        }

        private CreateProductService CreateService()
        {
            return new CreateProductService(_orderRepository);
        }

        [Fact] 
        public async Task CreateAsync_ProductExists_ShouldReturnProductEntity()
        {
            // Arrange
            var service = CreateService();

            var name = "newname";
            var description = "newdescription";
            var sku = "newsku";
            var product = new CreateProductRequest()
            {
                Name = name,
                Description = description,
                Sku = sku
            };

            // Act
            var result = await service.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be(description);
            result.SKU.Should().Be(sku);
            result.Name.Should().Be(name);
            _dbFixture.DbContext.Products.Should()
                .Contain(p => p.SKU == sku && p.Description == description && p.Name == name);
        }
    }
}
