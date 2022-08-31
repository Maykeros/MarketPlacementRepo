namespace MarketPlacement.Api.Automapper;

using AutoMapper;
using Domain.DTOs.Authentication;
using Domain.Entities.Authentication;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserEditDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<User, ProfileDto>();
    }
}