namespace PokerPlanning.Infrastructure.src.Authentication;

public class JwtSettings
{
    public static string SectionName = "JwtSettings";
    public required string Secret { get; init; }
    public int ExpiryMinutes { get; init; }
    public required string Issuer { get; init; }
}