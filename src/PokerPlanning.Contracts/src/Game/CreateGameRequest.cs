namespace PokerPlanning.Contracts.src.Game;

public record CreateGameRequest(
    string Name,
    Guid VotingSystemId,
    string CreatorName,
    bool IsAutoRevealCards = false
);
