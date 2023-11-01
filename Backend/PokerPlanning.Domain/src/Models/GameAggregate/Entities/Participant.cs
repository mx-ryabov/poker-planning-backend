using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class Participant : Entity<Guid>
{
    public Participant(Guid id) : base(id)
    {
    }

    public required string DisplayName { get; set; }
    public User? User { get; set; } = null;
    public required ParticipantRole Role { get; set; }
}
