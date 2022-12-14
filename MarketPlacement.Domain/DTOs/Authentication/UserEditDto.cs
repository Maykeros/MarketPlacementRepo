namespace MarketPlacement.Domain.DTOs.Authentication;

public class UserEditDto
{
    public int Id { get; set; }
        
    public string Email { get; set; }
    
    public string Password { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}