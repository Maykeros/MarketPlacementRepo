namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string UserName => Email;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}