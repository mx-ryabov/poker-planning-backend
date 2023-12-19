using PokerPlanning.Application.src.Common.Interfaces.Persistence;

namespace PokerPlanning.Infrastructure.src.Persistence.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly PokerPlanningDbContext _dbContext;

    public UnitOfWork(PokerPlanningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
