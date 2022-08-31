namespace MarketPlacement.Application;

using Domain.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Services.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAdminService, AdminService>();
        return services;
    }
}