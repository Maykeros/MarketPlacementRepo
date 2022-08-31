namespace MarketPlacement.Application.Services.Authentication;

using Domain.DTOs;
using Domain.DTOs.Authentication;
using Domain.Entities.Authentication;
using Domain.Repositories;
using Domain.ResultConstants;
using Domain.ResultConstants.Authorization;
using Domain.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Result;
using Result.Generic;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;

    private readonly IEfUnitOfWork _unitOfWork;

    public AdminService(UserManager<User> userManager, IEfUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public Task<Result<Pager<User>>> GetUsersPageAsync(string? search, int page, int items) 
        => _unitOfWork.UserRepository.GetUserPageAsync(search, page, items);

    public Task<Result<User>> GetUserAsync(int id) 
        => _unitOfWork.UserRepository.GetUserByIdAsync(id);

    public async Task<Result<User>> EditUserAsync(UserEditDto editUserDto)
    {
        try
        {
            var editUserResult = await _unitOfWork.UserRepository.EditUserAsync(editUserDto);

            if (!editUserResult.Success)
                return editUserResult;

            var userEntity = editUserResult.Data;
                
            var removePassword = await _userManager.RemovePasswordAsync(userEntity);

            if(!removePassword.Succeeded)
                return Result<User>.CreateFailed(AccountResultConstants.ErrorRemovingPassword);
                
            var addPass = await _userManager.AddPasswordAsync(userEntity, editUserDto.Password);

            return !addPass.Succeeded
                ? Result<User>.CreateFailed(AccountResultConstants.ErrorAddingPassword)
                : Result<User>.CreateSuccess(userEntity);
        }
        catch (Exception e)
        {
            return Result<User>.CreateFailed(CommonConstants.Unexpected, e);
        }
    }

    public Task<Result> DeleteUserAsync(int id) 
        => _unitOfWork.UserRepository.DeleteUserAsync(id);
}