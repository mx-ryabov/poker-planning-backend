namespace PokerPlanning.Application.src.Results;

public record UserResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);