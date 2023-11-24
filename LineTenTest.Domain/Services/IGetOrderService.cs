using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Domain.Services
{
    public interface IGetOrderService
    {
        Task<Order> GetAsync(int orderId);
    }
}
