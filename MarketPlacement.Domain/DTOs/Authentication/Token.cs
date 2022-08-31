namespace MarketPlacement.Domain.DTOs.Authentication;

public class Token
{
    public Token(string accessToken)
    {
        AccessToken = accessToken;
    }

    public string AccessToken { get;}
}