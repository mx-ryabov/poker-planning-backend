namespace PokerPlanning.Contracts.src.Authentication;

public record AuthenticationResponse(
    Guid id,
    string FirstName,
    string LastName,
    string Email,
    string Token // probably the best choice is to provide token through httpOnly cookies
);
