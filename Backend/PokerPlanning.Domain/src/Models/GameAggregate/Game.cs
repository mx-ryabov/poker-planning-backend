using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.TicketAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate;

public class Game : AggregateRoot<Guid>
{
    protected Game(Guid id) : base(id)
    {
    }

    public required string Name { get; set; }
    public required string Link { get; set; }
    public required GameSettings Settings { get; set; }
    public required VotingProcess VotingProcess { get; set; }
    public required VotingSystem VotingSystem { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public List<VotingResult> VotingResults { get; set; } = new List<VotingResult>();
}
