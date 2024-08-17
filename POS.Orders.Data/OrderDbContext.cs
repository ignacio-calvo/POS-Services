using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POS.Orders.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Orders.Data
{
    public class OrderDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Order> Orders { get; set; }
        public OrderDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("OrderDB"));
            optionsBuilder.EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(p => p.orderLines)
                .WithOne(s => s.Order)
                .HasForeignKey(s => s.Id);

            SeedOrders(modelBuilder);
        }


        private void SeedOrders(ModelBuilder modelBuilder)
        {

            Order ord = new Order
            {
                Id = 1,
                Date = DateTime.Now
            };

            OrderLine line1 = new OrderLine
            {
                OrderId = ord.Id,
                Id = 1,
                Sequence = 1,
                Quantity = 1,
                Price = 18.99m,
                ProductId = 1,
                ProductSizeId = 1
            };

            modelBuilder.Entity<Order>().HasData(ord);
            modelBuilder.Entity<OrderLine>().HasData(line1);
        }
    }
}
