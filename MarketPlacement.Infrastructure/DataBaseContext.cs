namespace MarketPlacement.Infrastructure;

using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataBaseContext : IdentityDbContext<User, Role, int>
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {
    }
}
