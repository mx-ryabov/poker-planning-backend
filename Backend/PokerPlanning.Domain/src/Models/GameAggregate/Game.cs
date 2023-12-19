using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.TicketAggregate;
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
    public VotingSystem VotingSystem { get; set; } = null!;
    public required Guid VotingSystemId { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public List<VotingResult> VotingResults { get; set; } = new List<VotingResult>();

    public static Game Create(string name, string link, GameSettings settings, Guid votingSystemId, Participant master)
    {
        var votingProcess = new VotingProcess();
        return new(Guid.NewGuid())
        {
            Name = name,
            Link = link,
            Settings = settings,
            VotingProcess = votingProcess,
            VotingSystemId = votingSystemId,
            Participants = new List<Participant>() { master }
        };
    }
}
