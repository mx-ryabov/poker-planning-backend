namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class GameSettings
{
    public bool IsAutoRevealCards { get; set; } = true;
    public int AutoRevealPeriod { get; set; } = 120;
}
