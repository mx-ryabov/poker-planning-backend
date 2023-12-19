namespace PokerPlanning.Contracts.src.Game;

public record GameRequest(
    string Name,
    Guid VotingSystemId,
    string CreatorName,
    bool IsAutoRevealCards = false
);
