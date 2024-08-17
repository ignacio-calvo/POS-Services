using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Customers.Data.Models;

namespace POS.Customers.Data
{
    public class CustomerDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Customer> Customers { get; set; }
        public CustomerDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CustomerDB"));
            optionsBuilder.EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                        
            modelBuilder.Entity<Customer>();

            SeedCustomers(modelBuilder);
        }


        private void SeedCustomers(ModelBuilder modelBuilder)
        {

            Customer cust01 = new Customer
            {
                Id = 1,
                FirstName = "Ignacio",
                LastName = "Calvo",
                PhoneNumber = "3513335304",
                Email = "ignaciocalvo@live.com.ar"
            };


            modelBuilder.Entity<Customer>().HasData(cust01);
        }
    }
}
