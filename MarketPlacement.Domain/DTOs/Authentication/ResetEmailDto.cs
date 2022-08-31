namespace MarketPlacement.Domain.DTOs.Authentication;

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

public class ResetEmailDto
{
    [DataType(DataType.EmailAddress)]
    public string OldEmail { get; }
    
    [DataType(DataType.EmailAddress)]
    public string NewEmail { get; }

    [DataMember]
    [Compare("NewEmail", ErrorMessage = "Emails have to be equal")]
    public string ConfirmNewEmail { get; }
}