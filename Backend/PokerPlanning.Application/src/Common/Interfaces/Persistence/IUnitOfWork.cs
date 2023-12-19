namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken);
}
