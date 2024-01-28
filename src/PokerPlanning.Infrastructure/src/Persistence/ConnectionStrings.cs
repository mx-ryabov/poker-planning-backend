namespace PokerPlanning.Infrastructure.src.Persistence;

public class ConnectionStrings
{
    public static string SectionName = "ConnectionStrings";
    public required string PokerPlanningDbConnection { get; init; }
}
