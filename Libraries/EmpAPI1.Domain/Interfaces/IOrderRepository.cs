using Menu.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetActiveOrdersByCustomerId(Guid customerId);
        Task CreateOrder(Order orderDto);
        Task<Order> GetOrderById(Guid orderId);
        Task<IEnumerable<Order>> GetUnpaidOrdersByCustomerId(Guid customerId);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
    }
}
