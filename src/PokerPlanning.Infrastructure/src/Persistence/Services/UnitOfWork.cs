using Microsoft.EntityFrameworkCore.Storage;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;

namespace PokerPlanning.Infrastructure.src.Persistence.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly PokerPlanningDbContext _dbContext;
    private IDbContextTransaction Transaction { get; set; } = null!;

    public UnitOfWork(PokerPlanningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginAsync(CancellationToken cancellationToken)
    {
        Transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await Transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await Transaction.RollbackAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
