namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;

public class ResetPasswordDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}