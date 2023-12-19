using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.GameAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Repositories;

public class GameRepository : IGameRepository
{
    private readonly PokerPlanningDbContext _dbContext;

    public GameRepository(PokerPlanningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(Game game, CancellationToken cancellationToken)
    {
        await _dbContext.Games.AddAsync(game, cancellationToken);
    }
}
