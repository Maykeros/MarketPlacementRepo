namespace MarketPlacement.Domain.Services.Authentication;

public interface IEmailService
{
    public Task SendAsync(string to, string body, string? subject);
}