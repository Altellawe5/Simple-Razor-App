using Menu.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CreateDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var optionsBuilder = new DbContextOptionsBuilder<MenuDbContext>();
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=RestaurantMenu;Integrated Security=True;TrustServerCertificate=True");
            using(var ctx =  new MenuDbContext(optionsBuilder.Options))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
            
        }
    }
}