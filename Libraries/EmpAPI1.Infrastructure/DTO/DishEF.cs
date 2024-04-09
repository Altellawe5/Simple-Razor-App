using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.DTO
{
    public class DishEF
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }
        
        public bool IsActive { get; set; }

        public List<OrderItemEF>? OrderItems { get; set; }
    }
}
