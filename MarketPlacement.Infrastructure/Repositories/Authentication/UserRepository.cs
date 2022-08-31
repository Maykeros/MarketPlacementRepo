namespace MarketPlacement.Infrastructure.Repositories.Authentication;

using Application.Extensions;
using Application.Result;
using Application.Result.Generic;
using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.Authentication;
using Domain.Entities.Authentication;
using Domain.Repositories.AuthenticationRepository;
using Domain.ResultConstants;
using Domain.ResultConstants.Authorization;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _ctx;
    private readonly IMapper _mapper;

    public UserRepository(DataBaseContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }



    public async Task<Result<Pager<User>>> GetUserPageAsync(string? search, int page, int count)
    {
        try
        {
            var totalCount = await _ctx.Users.CountAsync();

            var getPage = _ctx.Users
                .OrderBy(u => u.Id)
                .GetPage(page, count);

            if (!string.IsNullOrEmpty(search))
            {
                getPage = getPage.Where(u => u.Email.Contains(search) || u.UserName.Contains(search));
            }

            return Result<Pager<User>>.CreateSuccess(
                new Pager<User>(
                    await getPage.ToListAsync(),
                    totalCount));
        }
        catch (Exception e)
        {
            return Result<Pager<User>>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result<User>> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return Result<User>.CreateFailed(AccountResultConstants.UserNotFound);
            }

            return Result<User>.CreateSuccess(user);
        }
        catch (Exception e)
        {
            return Result<User>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result<User>> EditUserAsync(UserEditDto userDto)
    {
        try
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
            if (user is null)
            {
                return Result<User>.CreateFailed(AccountResultConstants.UserNotFound);
            }

            user.Email = userDto.Email;
            user.FirstName = userDto.Email;
            user.Lastname = userDto.LastName;

            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();

            var mappedUser = _mapper.Map<User>(userDto);
            return Result<User>.CreateSuccess(mappedUser);
        }
        catch (Exception e)
        {
            return Result<User>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return Result.CreateFailed(AccountResultConstants.UserNotFound);
            }

            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();

            return Result.CreateSuccess();
        }
        catch (Exception e)
        {
            return Result.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public async Task<Result<bool>> UserExistAsync(string email)
    {
        try
        {
            return Result<bool>.CreateSuccess(
                await _ctx.Users.AnyAsync(u => u.Email == email)
            );
        }
        catch (Exception e)
        {
            return Result<bool>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }
}