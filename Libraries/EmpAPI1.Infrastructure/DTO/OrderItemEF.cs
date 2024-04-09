using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.DTO
{
    public class OrderItemEF
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid DishId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public OrderEF? Order { get; set; }
        public DishEF? Dish { get; set; }
        public bool IsActive { get; set; }

    }
}
