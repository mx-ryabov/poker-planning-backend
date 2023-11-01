namespace PokerPlanning.Contracts.src.Authentication;

public record LoginRequest(
    string Email,
    string Password
);
