using Microsoft.AspNetCore.Mvc.Infrastructure;
using PokerPlanning.Api.Errors;

namespace PokerPlanning.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, PokerPlanningProblemDetailsFactory>();
        return services;
    }
}