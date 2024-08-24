using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Stores.Data.Models;

namespace POS.Stores.Data
{
    public class StoreDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Store> Stores { get; set; }
        public StoreDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("StoreDB"));
            optionsBuilder.EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                        
            modelBuilder.Entity<Store>();

            SeedStores(modelBuilder);
        }


        private void SeedStores(ModelBuilder modelBuilder)
        {

            Store store01 = new Store
            {
                Id = 1,
                Name = "Main Store",
                IsDefault = true,
                Email = "mainstore@tenant.com",
                PhoneNumber = "3510000000", 
                City = "Miami",
                State = "FL",
                PostalCode = "33101",
                AddressLine1 = "Main St",
                StreetNumber = "123"
            };


            modelBuilder.Entity<Store>().HasData(store01);
        }
    }
}
