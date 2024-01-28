namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task SaveAsync(CancellationToken cancellationToken);
    Task BeginAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}
