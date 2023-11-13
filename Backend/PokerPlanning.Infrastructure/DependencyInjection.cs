using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Infrastructure.src.Authentication;
using PokerPlanning.Infrastructure.src.Authentication.Services;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Infrastructure.src.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PokerPlanning.Infrastructure.src.Persistence;

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
        return services;
    }

    public static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStrings.SectionName).Get<ConnectionStrings>()?.PokerPlanningDbConnection;
        services.AddDbContext<PokerPlanningDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVotingSystemRepository, VotingSystemRepository>();
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
        return services;
    }
}
