using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IGameRepository
{
    Task<Game> Get(Guid gameId, CancellationToken cancellationToken);
    Task Create(Game game, CancellationToken cancellationToken);
    Task AddParticipant(Guid gameId, Participant participant, CancellationToken cancellationToken);
}
