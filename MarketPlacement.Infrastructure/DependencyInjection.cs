namespace MarketPlacement.Infrastructure;

using Authentication;
using Domain.Repositories;
using Domain.Repositories.AuthenticationRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Authentication;

public static class DependencyInjection
{

    public static IServiceCollection AddDataBaseConnection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddDbContext<DataBaseContext>(options =>
            options.UseSqlServer(configurationManager.GetConnectionString("DefaultConnection")));
        return services;
    }

    public static IServiceCollection AddJwtTokenGenerator(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
    
    public static IServiceCollection AddEfUnitOfWork(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, IUserRepository>();
        services.AddScoped<IEfUnitOfWork, EfUnitOfWork>();
        return services;
    }
}