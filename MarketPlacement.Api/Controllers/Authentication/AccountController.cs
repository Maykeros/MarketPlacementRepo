namespace MarketPlacement.Api.Controllers.Authentication;

using System.ComponentModel.DataAnnotations;
using Domain.DTOs.Authentication;
using Domain.Entities.Authentication;
using Domain.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly RoleManager<Role> _roleManager;

    public AccountController(IAccountService accountService, RoleManager<Role> roleManager)
    {
        _accountService = accountService;
        _roleManager = roleManager;
    }

    [HttpPost("roles")]
    public async Task<IActionResult> AddRoles()
    {
        if (await _roleManager.Roles.AnyAsync())
        {
            return NoContent();
        }
        await _roleManager.CreateAsync(new Role() {Name = "admin"});
        await _roleManager.CreateAsync(new Role() {Name = "user"});
        return Ok();
    }

    [HttpPost("register/token")]
    public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
    {
        return Ok(await _accountService.CreateUserAndSendEmailTokenAsync(registerDto));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> ConfirmRegistration([FromQuery]string token,[FromQuery] string userId)
    {
        return Ok(await _accountService.ConfirmRegistrationAsync(token, userId));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LogInUserDto userDto)
    {
        return Ok(await _accountService.GetAccessTokenAsync(userDto));
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(int userId)
    {
        return Ok(await _accountService.GetProfile(userId));
    }
    
    [HttpPost("email/token")]
    public async Task<IActionResult> ChangeEmail([FromBody] ResetEmailDto emailDto)
    {
        return Ok(await _accountService.SendEmailResetTokenAsync(emailDto));
    }
    
    [HttpPut("email")]
    public async Task<IActionResult> ConfirmChangingEmail(string token, string newEmail, string userId)
    {
        return Ok(await _accountService.ResetEmailAsync(token,newEmail,userId));
    }
    
    [HttpPost("password/token")]
    public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordDto passwordDto)
    {
        return Ok(await _accountService.SendPasswordResetTokenAsync(passwordDto));
    }
    
    [HttpDelete("password")]
    public async Task<IActionResult> ConfirmChangingPassword([FromBody] TokenPasswordDto passwordDto)
    {
        return Ok(await _accountService.ResetPasswordAsync(passwordDto));
    }
}
