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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var entity = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)  
                .FirstOrDefaultAsync(c => c.Id == id);
            return entity == null ? null : _mapper.Map<Customer>(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var entities = await _context.Customers.Where(c => c.IsActive).ToListAsync();
            return _mapper.Map<IEnumerable<Customer>>(entities);
        }
        public async Task AddAsync(Customer customer)
        {
            var entity = _mapper.Map<CustomerEF>(customer);
            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var entity = _mapper.Map<CustomerEF>(customer);
            entity.IsActive = true;
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsActive = false;
                _context.Entry(customer).State = EntityState.Modified;
            }            
            await _context.SaveChangesAsync();

           
        }

        public async Task<Customer> GetByNameAsync(string firstName, string lastName)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.FirstName == firstName && c.LastName == lastName && c.IsActive);
            return customer == null ? null : _mapper.Map<Customer>(customer);
        }
    }
}
