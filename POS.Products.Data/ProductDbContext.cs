using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace POS.Products.Data.Models
{
    public class ProductDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public ProductDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ProductDB"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sizes)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            SeedProducts(modelBuilder);
        }

        private void SeedProducts(ModelBuilder modelBuilder)
        {
            // Seed data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Pizzas", Description = "Variety of pizzas", ImageUrl = "pizza-icon.png" },
                new Category { Id = 2, Name = "Sides", Description = "Side dishes like cheese sticks", ImageUrl = "sides-icon.png" },
                new Category { Id = 3, Name = "Beverages", Description = "Drinks", ImageUrl = "beverages-icon.png" },
                new Category { Id = 4, Name = "Chicken Wings", Description = "Variety of chicken wings", ImageUrl = "chickenwings-icon.png" },
                new Category { Id = 5, Name = "Pasta", Description = "Different types of pasta", ImageUrl = "pasta-icon.png" },
                new Category { Id = 6, Name = "Salad", Description = "Fresh salads", ImageUrl = "salad-icon.png" }
            );

            // Seed data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Margherita Pizza", Description = "Classic pizza with tomato, mozzarella, and basil", DisplayOrder = 1, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "margherita.png" },
                new Product { Id = 2, Name = "Pepperoni Pizza", Description = "Pizza with pepperoni and cheese", DisplayOrder = 2, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "pepperoni.png" },
                new Product { Id = 3, Name = "Cheese Sticks", Description = "Baked cheese sticks", DisplayOrder = 3, StatusCode = 1, ProductTypeCode = 2, ProductImageUrl = "cheesesticks.png" },
                new Product { Id = 4, Name = "Coca-Cola", Description = "Soft drink", DisplayOrder = 4, StatusCode = 1, ProductTypeCode = 3, ProductImageUrl = "cocacola.png" },
                new Product { Id = 5, Name = "BBQ Chicken Wings", Description = "Chicken wings with BBQ sauce", DisplayOrder = 5, StatusCode = 1, ProductTypeCode = 4, ProductImageUrl = "bbqwings.png" },
                new Product { Id = 6, Name = "Spaghetti Bolognese", Description = "Spaghetti with meat sauce", DisplayOrder = 6, StatusCode = 1, ProductTypeCode = 5, ProductImageUrl = "spaghetti.png" },
                new Product { Id = 7, Name = "Caesar Salad", Description = "Salad with romaine lettuce, croutons, and Caesar dressing", DisplayOrder = 7, StatusCode = 1, ProductTypeCode = 6, ProductImageUrl = "caesarsalad.png" },
                new Product { Id = 8, Name = "Hawaiian Pizza", Description = "Pizza with ham and pineapple", DisplayOrder = 8, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "hawaiian.png" },
                new Product { Id = 9, Name = "Veggie Pizza", Description = "Pizza with various vegetables", DisplayOrder = 9, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "veggie.png" },
                new Product { Id = 10, Name = "BBQ Chicken Pizza", Description = "Pizza with BBQ chicken", DisplayOrder = 10, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "bbqchicken.png" },
                new Product { Id = 11, Name = "Buffalo Chicken Pizza", Description = "Pizza with buffalo chicken", DisplayOrder = 11, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "buffalochicken.png" },
                new Product { Id = 12, Name = "Meat Lovers Pizza", Description = "Pizza with various meats", DisplayOrder = 12, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "meatlovers.png" },
                new Product { Id = 13, Name = "Supreme Pizza", Description = "Pizza with various toppings", DisplayOrder = 13, StatusCode = 1, ProductTypeCode = 1, ProductImageUrl = "supreme.png" }
            );

            // Seed data for ProductSizes
            modelBuilder.Entity<ProductSize>().HasData(
                new ProductSize { Id = 1, ProductId = 1, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 8.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 2, ProductId = 1, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 10.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 3, ProductId = 1, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 12.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 4, ProductId = 2, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 5, ProductId = 2, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 6, ProductId = 2, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 7, ProductId = 3, Name = "Regular", DisplayOrder = 1, StatusCode = 1, Price = 4.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 8, ProductId = 4, Name = "Can", DisplayOrder = 1, StatusCode = 1, Price = 1.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 9, ProductId = 5, Name = "6 Pieces", DisplayOrder = 1, StatusCode = 1, Price = 6.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 10, ProductId = 5, Name = "12 Pieces", DisplayOrder = 2, StatusCode = 1, Price = 12.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 11, ProductId = 6, Name = "Regular", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 12, ProductId = 7, Name = "Regular", DisplayOrder = 1, StatusCode = 1, Price = 7.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 13, ProductId = 8, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 14, ProductId = 8, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 15, ProductId = 8, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 16, ProductId = 9, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 17, ProductId = 9, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 18, ProductId = 9, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 19, ProductId = 10, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 20, ProductId = 10, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 21, ProductId = 10, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 22, ProductId = 11, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 23, ProductId = 11, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 24, ProductId = 11, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 25, ProductId = 12, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 26, ProductId = 12, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 27, ProductId = 12, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 28, ProductId = 13, Name = "Small", DisplayOrder = 1, StatusCode = 1, Price = 9.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 29, ProductId = 13, Name = "Medium", DisplayOrder = 2, StatusCode = 1, Price = 11.99m, DefaultSize = true, PriceByWeight = false, TareWeight = 0 },
                new ProductSize { Id = 30, ProductId = 13, Name = "Large", DisplayOrder = 3, StatusCode = 1, Price = 13.99m, DefaultSize = false, PriceByWeight = false, TareWeight = 0 }
            );

            // Seed data for ProductCategory (many-to-many relationship)
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 1 },
                new ProductCategory { ProductId = 3, CategoryId = 2 },
                new ProductCategory { ProductId = 4, CategoryId = 3 },
                new ProductCategory { ProductId = 5, CategoryId = 4 },
                new ProductCategory { ProductId = 6, CategoryId = 5 },
                new ProductCategory { ProductId = 7, CategoryId = 6 },
                new ProductCategory { ProductId = 8, CategoryId = 1 },
                new ProductCategory { ProductId = 9, CategoryId = 1 },
                new ProductCategory { ProductId = 10, CategoryId = 1 },
                new ProductCategory { ProductId = 11, CategoryId = 1 },
                new ProductCategory { ProductId = 12, CategoryId = 1 },
                new ProductCategory { ProductId = 13, CategoryId = 1 }
            );
        }
    }
}
