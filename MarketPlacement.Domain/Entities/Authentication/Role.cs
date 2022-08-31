namespace MarketPlacement.Domain.Entities.Authentication;

using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<int>
{
    public string? RoleDescription { get; set; }
  
}