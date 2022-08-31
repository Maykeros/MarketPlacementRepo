namespace MarketPlacement.Domain.Entities.Authentication;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
    [Required,EmailAddress]
    public override string Email { get; set; }

    public string FirstName { get; set; }

    public string Lastname { get; set; }
}