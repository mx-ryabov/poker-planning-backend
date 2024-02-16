using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.TestUtils.ModelUtils.Constants;

public static class GameConstants
{
    public static string Name => "Game's Name";
    public static string Link => "01ea7375-aeee-489c-afe7-f6dfa93b5407";
    public static GameSettings Settings => new();
    public static string VotingSystemId => "103ac41a-8a47-4ec3-ba56-8344028e7d22";
}
