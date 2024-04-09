using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.DTO
{
    public class OrderEF
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        // Navigation property
        public CustomerEF? Customer { get; set; }

        // Navigation property
        public List<OrderItemEF>? OrderItems { get; set; }
    }
}
