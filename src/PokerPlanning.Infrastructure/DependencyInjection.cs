using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Infrastructure.src.Authentication;
using PokerPlanning.Infrastructure.src.Services;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Infrastructure.src.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PokerPlanning.Infrastructure.src.Persistence;
using PokerPlanning.Infrastructure.src.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Quartz;

namespace PokerPlanning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistance(configuration);
        services.AddConfigurations(configuration);
        services.AddServices();
        services.AddIdentity(configuration);
        services.AddCors();
        return services;
    }

    public static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStrings.SectionName).Get<ConnectionStrings>()?.PokerPlanningDbConnection;
        services.AddDbContext<PokerPlanningDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVotingSystemRepository, VotingSystemRepository>();
        services.AddScoped<IEmailCollectionRepository, EmailCollectionRepository>();
        return services;
    }

    public static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddQuartz();
        services.AddSingleton<ITaskScheduler, CustomTaskScheduler>();
        services.AddScoped<IGameTimer, GameTimer>();
        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

        services.AddIdentity<ApplicationUser, ApplicationUserRole>()
            .AddRoles<ApplicationUserRole>()
            .AddEntityFrameworkStores<PokerPlanningDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = jwtSettings?.Audience ?? "",
                    ValidIssuer = jwtSettings?.Issuer ?? "",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? ""))
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        return services;
    }
}
