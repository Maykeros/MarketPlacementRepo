namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;

public class ProfileDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public int Id { get; set; }
}