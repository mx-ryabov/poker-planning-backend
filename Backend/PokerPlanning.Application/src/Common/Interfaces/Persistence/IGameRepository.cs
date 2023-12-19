using PokerPlanning.Domain.src.Models.GameAggregate;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IGameRepository
{
    Task Create(Game game, CancellationToken cancellationToken);
}
