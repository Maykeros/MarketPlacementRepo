namespace MarketPlacement.Infrastructure;

using Domain.Entities.Authentication;
using Domain.Entities.CoreEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataBaseContext : IdentityDbContext<User, Role, int>
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }
        
}
