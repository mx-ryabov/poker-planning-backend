using Microsoft.Extensions.DependencyInjection;
using PokerPlanning.Infrastructure.src.Persistence;

namespace PokerPlanning.Api.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<PokerPlanningWebApplicationFactory>
{
    protected readonly HttpClient _client;
    protected readonly PokerPlanningDbContext _dbContext;

    private readonly PokerPlanningWebApplicationFactory _factory;

    protected BaseIntegrationTest(PokerPlanningWebApplicationFactory factory)
    {
        Environment.SetEnvironmentVariable("IsTesting", "true");
        _factory = factory;
        _client = _factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<PokerPlanningDbContext>();
    }
}
