using System.Text;
using MarketPlacement.Application;
using MarketPlacement.Application.Extensions;
using MarketPlacement.Application.Services.Authentication;
using MarketPlacement.Domain.Entities.Authentication;
using MarketPlacement.Domain.Repositories;
using MarketPlacement.Domain.Repositories.AuthenticationRepository;
using MarketPlacement.Domain.Services.Authentication;
using MarketPlacement.Infrastructure;
using MarketPlacement.Infrastructure.Authentication;
using MarketPlacement.Infrastructure.Repositories;
using MarketPlacement.Infrastructure.Repositories.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataBaseConnection(builder.Configuration);

builder.Services.AddJwtTokenGenerator(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEfUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services
    .AddIdentity<User, Role>(options => options.PasswordSettings())
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders();


var configurationSettings = new JwtSettings();
builder.Configuration.Bind("JwtSettings", configurationSettings);
builder.Services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            
            ValidIssuer = configurationSettings.Issuer,
            ValidAudience = configurationSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationSettings.Secret)),
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "JWTToken_Auth_API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();