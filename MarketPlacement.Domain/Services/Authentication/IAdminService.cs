namespace MarketPlacement.Domain.Services.Authentication;

using System.ComponentModel.DataAnnotations;
using Application.Result;
using Application.Result.Generic;
using DTOs;
using DTOs.Authentication;
using Entities.Authentication;

public interface IAdminService
{
    Task<Result<Pager<User>>> GetUsersPageAsync(string? search, int page, int items);

    Task<Result<User>> GetUserAsync([Range(0, int.MaxValue)] int id);

    Task<Result<User>> EditUserAsync(UserEditDto editUserDto);

    Task<Result> DeleteUserAsync([Range(0, int.MaxValue)] int id);
}