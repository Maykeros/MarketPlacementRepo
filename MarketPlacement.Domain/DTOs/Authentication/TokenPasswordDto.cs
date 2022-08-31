namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

public class TokenPasswordDto
{
    [DataType(DataType.EmailAddress)] 
    public string Email { get; }

    [DataType(DataType.Password)] 
    public string NewPassword { get; }

    [DataMember]
    [Compare("NewPassword", ErrorMessage = "Passwords have to be equal")]
    public string ConfirmNewPassword { get; }

    public string Token { get; }
}