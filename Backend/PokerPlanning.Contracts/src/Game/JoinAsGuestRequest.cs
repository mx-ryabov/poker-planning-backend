namespace PokerPlanning.Contracts.src.Game;

public record JoinAsGuestRequest(
    Guid GameId,
    string DisplayName
);
