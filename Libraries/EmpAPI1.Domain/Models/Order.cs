using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
