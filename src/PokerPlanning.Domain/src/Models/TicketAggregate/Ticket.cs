using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Common.DTO;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Domain.src.Models.TicketAggregate;

public class Ticket : AggregateRoot<Guid>
{
    protected Ticket(Guid id) : base(id)
    {
    }

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = "";
    public string? Link { get; private set; }
    public TicketType? Type { get; private set; }
    public string? Identifier { get; private set; }
    public string? Estimation { get; private set; }
    public Game? Game { get; private set; }
    public Guid? GameId { get; private set; }

    public static Ticket CreateNew(string title, TicketType type, Guid? gameId)
    {
        return new(Guid.NewGuid())
        {
            Title = title,
            Type = type,
            GameId = gameId,
        };
    }

    public void Update(UpdateTicketDTO data)
    {
        Title = data.Title ?? Title;
        Description = data.Description ?? Description;
        Link = data.Link ?? Link;
        Type = data.Type ?? Type;
        Identifier = data.Identifier ?? Identifier;
        Estimation = data.Estimation ?? Estimation;
    }
}
