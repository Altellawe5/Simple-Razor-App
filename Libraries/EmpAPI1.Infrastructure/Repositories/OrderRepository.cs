using AutoMapper;
using Menu.Domain.Interfaces;
using Menu.Domain.Models;
using Menu.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Order>> GetActiveOrdersByCustomerId(Guid customerId)
        {
            var orderEFs = await _context.Orders
                .Where(o => o.CustomerId == customerId && o.IsActive)
                .Include(o=> o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Dish)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Order>>(orderEFs);
        }

        public async Task<IEnumerable<Order>> GetUnpaidOrdersByCustomerId(Guid customerId)
        {
            var orderEFs = await _context.Orders
                .Where(o => o.CustomerId == customerId && o.PaymentDate == null && o.IsActive)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Dish)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Order>>(orderEFs);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEF = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            // Map the OrderEF object to an Order object and return
            return _mapper.Map<Order>(orderEF);
        }
        public async Task UpdateOrder(Order order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == order.Id);


            _mapper.Map(order, existingOrder);


            foreach (var item in existingOrder.OrderItems)
            {
                _context.Entry(item.Dish).State = EntityState.Unchanged;
            }

            await _context.SaveChangesAsync();
        }
        public async Task CreateOrder(Order order)
        {
            try
            {
                var orderEF = _mapper.Map<OrderEF>(order);
                orderEF.IsActive = true;             
                foreach (var item in orderEF.OrderItems)
                {
                    item.OrderId = orderEF.Id;
                    item.IsActive = true;
                    var dish = await _context.Dishes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == item.DishId);
                    if (dish != null)
                    {
                        item.Dish = dish;
                        _context.Entry(dish).State = EntityState.Unchanged;
                    }

                }
                await _context.Orders.AddAsync(orderEF);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        public async Task DeleteOrder(Order order)
        {
            var entity = await _context.Orders.FindAsync(order.Id);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }            
            foreach (var item in order.OrderItems)
            {
                var ItemEF = await _context.OrderItems.FindAsync(item.Id);
                if(ItemEF != null)
                {
                    ItemEF.IsActive = false;
                    await _context.SaveChangesAsync();
                }
            }            
        }

    }
}
