namespace MarketPlacement.Domain.Repositories.AuthenticationRepository;

using Application.Result;
using Application.Result.Generic;
using DTOs;
using DTOs.Authentication;
using Entities.Authentication;

public interface IUserRepository
{
    
    public Task<Result<Pager<User>>> GetUserPageAsync(string? search, int page, int count);

    public Task<Result<User>> GetUserByIdAsync(int userId);
    
    public Task<Result<User>> EditUserAsync(UserEditDto user);

    public Task<Result> DeleteUserAsync(int userId);

    public Task<Result<bool>> UserExistAsync(string email);
}