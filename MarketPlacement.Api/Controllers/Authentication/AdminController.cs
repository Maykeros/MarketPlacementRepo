namespace MarketPlacement.Api.Controllers.Authentication;

using System.ComponentModel.DataAnnotations;
using Domain.DTOs.Authentication;
using Domain.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersPage(
        [FromQuery] string? search,
        [FromQuery] [Range(1, int.MaxValue)] int page = 1,
        [FromQuery] int items = 5
    )
        => Ok(await _adminService.GetUsersPageAsync(search, page, items));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser([Range(0, int.MaxValue)] int id)
        => Ok(await _adminService.GetUserAsync(id));

    [HttpPut]
    public async Task<IActionResult> EditUsers(UserEditDto userDto) 
        => Ok(await _adminService.EditUserAsync(userDto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUsers([Range(0, int.MaxValue)] int id) 
        => Ok(await _adminService.DeleteUserAsync(id));
}