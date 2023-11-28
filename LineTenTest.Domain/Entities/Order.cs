using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Entities
{
    public class Order : IDbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public Product Product { get; set; } = new Product();
        public Customer Customer { get; set; } = new Customer();
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
