namespace MarketPlacement.Infrastructure.Authentication;

using Domain.Entities.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateToken(User user, string role);
}