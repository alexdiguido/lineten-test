using LineTenTest.Domain.Entities;

namespace LineTenTest.TestUtilities
{
    public class OrderBuilder
    {
        public static Order CreateDefault()
        {
            return new Order()
            {
                Id = 1,
                Status = (int)EOrderStatus.Pending,
                CreatedDate = new DateTime(2023, 11, 26, 11, 12, 13),
                UpdatedDate = new DateTime(2023, 11, 26, 11, 12, 13),
                Customer = CustomerBuilder.CreateDefault(),
                Product = ProductBuilder.CreateDefault()
            };
        }
    }
}