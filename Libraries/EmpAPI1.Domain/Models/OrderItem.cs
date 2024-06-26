﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public string? DishName { get; set; }
        public decimal? Price { get; set; }
    }
}
