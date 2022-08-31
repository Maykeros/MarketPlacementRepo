namespace MarketPlacement.Application.Services.Authentication;

using System.Net;
using System.Net.Mail;
using Domain.Services.Authentication;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
         _smtpClient = new SmtpClient("smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("b0167b4723ef68", "c07a081c5fb54d"),
            EnableSsl = true
        };
    }

    public Task SendAsync(string to, string body, string? subject) 
        => _smtpClient
            .SendMailAsync(
                _configuration["EmailSettings:Email"],
                to,
                subject ?? string.Empty,
                body
            );
}