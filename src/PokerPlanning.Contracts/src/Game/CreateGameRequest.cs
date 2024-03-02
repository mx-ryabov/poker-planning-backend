using PokerPlanning.Contracts.src.Common;

namespace PokerPlanning.Contracts.src.Game;

public record CreateGameRequest(
    string Name,
    [NotEmpty] Guid VotingSystemId,
    string CreatorName,
    bool IsAutoRevealCards = false
);
