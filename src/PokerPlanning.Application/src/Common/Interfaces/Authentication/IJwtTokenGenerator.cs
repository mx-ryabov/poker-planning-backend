namespace PokerPlanning.Application.src.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(string displayName, Guid guestId, string role);
}