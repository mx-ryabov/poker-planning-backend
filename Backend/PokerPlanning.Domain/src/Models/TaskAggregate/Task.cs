using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.TaskAggregate.Enums;

namespace PokerPlanning.Domain.src.Models.TaskAggregate;

public class Task : AggregateRoot<Guid>
{
    protected Task(Guid id) : base(id)
    {
    }

    public required string Title { get; set; }
    public string? Link { get; set; } = null;
    public TaskType? Type { get; set; } = null;
    public string? Identifier { get; set; } = null;
    public string Description { get; set; } = "";
    public string? Estimation { get; set; } = null;
    public Game? Game { get; set; } = null;
}
