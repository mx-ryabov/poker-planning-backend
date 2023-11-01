namespace PokerPlanning.Application.src.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName);
}