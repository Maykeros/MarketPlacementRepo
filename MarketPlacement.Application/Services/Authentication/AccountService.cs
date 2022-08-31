namespace MarketPlacement.Application.Services.Authentication;

using System.Web;
using AutoMapper;
using Domain.DTOs.Authentication;
using Domain.Entities.Authentication;
using Domain.Repositories;
using Domain.ResultConstants;
using Domain.ResultConstants.Authorization;
using Domain.Services.Authentication;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Result;
using Result.Generic;

public class AccountService : IAccountService
{
    private readonly IEfUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AccountService(IEfUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IEmailService emailService,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _emailService = emailService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }


    public async Task<Result> CreateUserAndSendEmailTokenAsync(RegisterDto register)
    {
        try
        {
            var userEntity = _mapper.Map<User>(register);

            if ((await _unitOfWork.UserRepository.UserExistAsync(register.Email)).Data)
                return Result.CreateFailed(AccountResultConstants.UserAlreadyExists);

            var credentialSucceed = await _userManager.CreateAsync(userEntity, register.Password);

            if (!credentialSucceed.Succeeded)
            {
                return Result.CreateFailed(AccountResultConstants.ErrorCreatingUser);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);

            var link = $"http://localhost:5000/api/account/register?token={token}&userId={userEntity.Id}";

            await _emailService.SendAsync(
                to: userEntity.Email,
                body: link,
                subject: "Confirm your registration");
            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> ConfirmRegistrationAsync(string token, string userId)
    {
        try
        {
            var userEntity = await _userManager.FindByIdAsync(userId);

            var tokenIsValid = await _userManager.ConfirmEmailAsync(userEntity, token);

            if (!tokenIsValid.Succeeded)
                return Result.CreateFailed(AccountResultConstants.InvalidRegistrationToken);

            await _userManager.AddToRoleAsync(userEntity, "user");
            
            
            await _emailService.SendAsync(
                to: userEntity.Email,
                body: "You have successfully created your account in library!",
                subject: "Registration confirmed"
            );

            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result<Token>> GetAccessTokenAsync(LogInUserDto userInput)
    {
        try
        {
            var userEntity = await _userManager.FindByEmailAsync(userInput.Email);

            if (userEntity is null)
            {
                return Result<Token>.CreateFailed(AccountResultConstants.UserNotFound);
            }

            var passwordIsValid = await _userManager.CheckPasswordAsync(userEntity, userInput.Password);

            var roles = await _userManager.GetRolesAsync(userEntity);

            var role = roles.FirstOrDefault(r => true);

            if (!passwordIsValid)
            {
                return Result<Token>.CreateFailed("Password is incorrect");
            }

            var accessToken = _jwtTokenGenerator.GenerateToken(userEntity, role!);

            return Result<Token>.CreateSuccess(new Token(accessToken));
        }
        catch (Exception e)
        {
            return Result<Token>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result<ProfileDto>> GetProfile(int userId)
    {
        try
        {
            var userEntity = await _userManager.FindByIdAsync(userId.ToString());

            return Result<ProfileDto>.CreateSuccess(_mapper.Map<ProfileDto>(userEntity));
        }
        catch (Exception e)
        {
            return Result<ProfileDto>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> SendEmailResetTokenAsync(ResetEmailDto resetEmailDto)
    {
        try
        {
            var userEntity = await _userManager.FindByEmailAsync(resetEmailDto.OldEmail);

            if (userEntity is null)
            {
                return Result.CreateFailed(AccountResultConstants.UserNotFound);
            }

            var resetEmailToken = HttpUtility.UrlEncode
                (await _userManager.GenerateChangeEmailTokenAsync(userEntity, resetEmailDto.NewEmail));

            var pureLink = "https://localhost:5000/api/account/reset-email" +
                           $"?token={resetEmailToken}" +
                           $"&newEmail={resetEmailDto.NewEmail}";

            await _emailService.SendAsync(
                to: resetEmailDto.OldEmail,
                body: pureLink,
                subject: "confirm your changing Email");

            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> ResetEmailAsync(string token, string newEmail, string userId)
    {
        try
        {
            var userEntity = await _userManager
                .FindByIdAsync(userId);

            if (userEntity is null)
                return Result.CreateFailed(
                    AccountResultConstants.UserNotFound,
                    new NullReferenceException()
                );

            var changeEmail = await _userManager
                .ChangeEmailAsync(userEntity, newEmail, token);

            if (!changeEmail.Succeeded)
                return Result.CreateFailed(AccountResultConstants.InvalidResetEmailToken);

            userEntity.UserName = newEmail;

            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> SendPasswordResetTokenAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var userEntity = await _userManager
                .FindByEmailAsync(resetPasswordDto.Email);

            if (userEntity is null)
                return Result.CreateFailed(
                    AccountResultConstants.UserNotFound,
                    new NullReferenceException()
                );

            var passwordResetToken = await _userManager
                .GeneratePasswordResetTokenAsync(userEntity);

            await _emailService.SendAsync(
                resetPasswordDto.Email,
                passwordResetToken,
                "Confirm reset your password"
            );

            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> ResetPasswordAsync(TokenPasswordDto tokenPasswordDto)
    {
        try
        {
            var userEntity = await _userManager
                .FindByEmailAsync(tokenPasswordDto.Email);

            if (userEntity is null)
                return Result.CreateFailed(
                    AccountResultConstants.UserNotFound,
                    new NullReferenceException()
                );

            var resetPassword = await _userManager.ResetPasswordAsync(
                userEntity,
                $"{tokenPasswordDto}",
                tokenPasswordDto.NewPassword
            );

            return !resetPassword.Succeeded
                ? Result.CreateFailed(AccountResultConstants.InvalidResetPasswordToken)
                : Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }
}