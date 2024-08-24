using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS.Identity.API.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
            new IdentityRole { Id = "1", Name = AppRoles.SuperAdmin, NormalizedName = "SUPERADMIN" },
            new IdentityRole { Id = "2", Name = AppRoles.Admin, NormalizedName = "ADMIN" },
            new IdentityRole { Id = "3", Name = AppRoles.Customer, NormalizedName = "CUSTOMER" },
            new IdentityRole { Id = "4", Name = AppRoles.Employee, NormalizedName = "EMPLOYEE" }
            
        );

        // Seed admin user
        var adminUser = new ApplicationUser
        {
            Id = "1", // Primary key
            UserName = "superadmin@positive.com",
            NormalizedUserName = "SUPERADMIN@POSITIVE.COM",
            Email = "superadmin@positive.com",
            NormalizedEmail = "SUPERADMIN@POSITIVE.COM",
            FirstName = "POSitive",
            LastName = "SuperAdmin",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "SuperAdmin@123");

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // Assign superadmin role to superadmin user
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1", // SuperAdmin role ID
                UserId = "1"  // Admin user ID
            }
        );
    }
}
