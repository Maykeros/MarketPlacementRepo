namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;

public class LogInUserDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}