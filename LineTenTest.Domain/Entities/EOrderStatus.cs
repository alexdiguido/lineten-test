using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineTenTest.Domain.Entities
{
    public enum EOrderStatus
    {
        Pending = 0,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Canceled = 5,
        Returned = 6,
        OnHold = 7,
        PaymentFailed = 8,
        Refunded = 9,
        PartiallyShipped=10,
    }
}
