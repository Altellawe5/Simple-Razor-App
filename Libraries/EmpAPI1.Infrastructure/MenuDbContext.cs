using Menu.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Infrastructure
{

    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerEF> Customers { get; set; }
        public DbSet<DishEF> Dishes { get; set; }
        public DbSet<OrderEF> Orders { get; set; }
        public DbSet<OrderItemEF> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEF>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<OrderEF>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItemEF>()
                .HasOne(oi => oi.Dish)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(oi => oi.DishId);

            modelBuilder.Entity<DishEF>()
                .Property(d => d.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<CustomerEF>()
                .Property(d => d.IsActive)
                .HasDefaultValue(true);


            modelBuilder.Entity<OrderEF>()
                .Property(d => d.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<OrderItemEF>()
                .Property(d => d.IsActive)
                .HasDefaultValue(true);
        }

    }
}