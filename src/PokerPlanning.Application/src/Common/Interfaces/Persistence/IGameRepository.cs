using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IGameRepository
{
    Task<Game?> Get(Guid gameId, CancellationToken cancellationToken);
    Task Create(Game game, CancellationToken cancellationToken);
    Task CreateParticipant(Participant participant, CancellationToken cancellationToken);
    Task<Participant?> GetParticipant(Guid gameId, Guid userId, CancellationToken cancellationToken);
    Task<ParticipantRole?> GetParticipantRole(Guid gameId, Guid userId, CancellationToken cancellationToken);
}
