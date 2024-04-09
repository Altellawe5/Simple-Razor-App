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
    public class DishRepository: IDishRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public DishRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Dish> GetByIdAsync(Guid id)
        {
            var entity = await _context.Dishes.FindAsync(id);
            return _mapper.Map<Dish>(entity);
        }

        public async Task<IEnumerable<Dish>> GetAllAsync()
        {
            var entities = await _context.Dishes.Where(d => d.IsActive).ToListAsync();
            return _mapper.Map<IEnumerable<Dish>>(entities);
        }

        public async Task AddAsync(Dish dish)
        {
            var entity = _mapper.Map<DishEF>(dish);
            await _context.Dishes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Dish dish)
        {
            
            var entity = _mapper.Map<DishEF>(dish);
            entity.IsActive = true;
            _context.Dishes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Dish dish)
        {
            var entity = await _context.Dishes.FindAsync(dish.Id);
            if (entity != null)
            {
                entity.IsActive = false;
                _context.Dishes.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
