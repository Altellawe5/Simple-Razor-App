using Menu.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish> GetByIdAsync(Guid id);
        Task<IEnumerable<Dish>> GetAllAsync();
        Task AddAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task DeleteAsync(Dish dish);
    }
}
