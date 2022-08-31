namespace MarketPlacement.Domain.Services.Authentication;

using Application.Result;
using Application.Result.Generic;
using DTOs.Authentication;

public interface IAccountService
{
    Task<Result> CreateUserAndSendEmailTokenAsync(RegisterDto register);

    Task<Result> ConfirmRegistrationAsync(string token, string userId);
        
    Task<Result<Token>> GetAccessTokenAsync(LogInUserDto userInput);

    Task<Result<ProfileDto>> GetProfile(int userId);

    Task<Result> SendEmailResetTokenAsync(ResetEmailDto resetEmailDto);

    Task<Result> ResetEmailAsync(string token, string newEmail, string oldEmail);

    Task<Result> SendPasswordResetTokenAsync(ResetPasswordDto resetPasswordDto);

    Task<Result> ResetPasswordAsync(TokenPasswordDto tokenPasswordDto);
}