using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewWebShopApp.Models;

namespace NewWebShopApp.Areas.Identity.Data;

public class AppDBContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "r1",
            Name = "admin",
            NormalizedName = "ADMIN"
        });

        builder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "a1",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassword"),
            SecurityStamp = string.Empty
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            UserId = "a1",
            RoleId = "r1"
        });


    }
}
