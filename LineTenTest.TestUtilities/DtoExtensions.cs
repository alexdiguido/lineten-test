using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Entities;

namespace LineTenTest.TestUtilities
{
    public static class DtoExtensions
    {
        public static bool ShouldBeEquivalentTo(this OrderDto dto, Order orderEntity)
        {
            dto.OrderId.Should().Be(orderEntity.Id);
            dto.CreatedDate.Should().Be(orderEntity.CreatedDate);
            dto.UpdateDate.Should().Be(orderEntity.UpdatedDate);
            dto.Status.Should().Be(orderEntity.Status);
            dto.Customer.ShouldBeEquivalentTo(orderEntity.Customer);
            dto.Product.ShouldBeEquivalentTo(orderEntity.Product);

            return true;
        }

        public static bool ShouldBeEquivalentTo(this CustomerDto customerDto, Customer customerEntity)
        {
            customerDto.Email.Should().Be(customerEntity.Email);
            customerDto.FirstName.Should().Be(customerEntity.FirstName);
            customerDto.LastName.Should().Be(customerEntity.LastName);
            customerDto.Phone.Should().Be(customerEntity.Phone);

            return true;
        }

        public static bool ShouldBeEquivalentTo(this ProductDto productDto, Product productEntity)
        {
            productDto.Description.Should().Be(productEntity.Description);
            productDto.Id.Should().Be(productEntity.Id);
            productDto.Name.Should().Be(productEntity.Name);
            productDto.SKU.Should().Be(productEntity.SKU);

            return true;
        }
    }
}
