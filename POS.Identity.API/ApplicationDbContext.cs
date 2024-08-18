using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS.Identity.API.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Call the SeedData method to seed roles and admin user
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );

        // Seed admin user
        var adminUser = new ApplicationUser
        {
            Id = "1", // Primary key
            UserName = "admin@positive.com",
            NormalizedUserName = "ADMIN@POSITIVE.COM",
            Email = "admin@positive.com",
            NormalizedEmail = "ADMIN@POSITIVE.COM",
            FirstName = "POSitive",
            LastName = "Admin",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // Assign admin role to admin user
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1", // Admin role ID
                UserId = "1"  // Admin user ID
            }
        );
    }
}
