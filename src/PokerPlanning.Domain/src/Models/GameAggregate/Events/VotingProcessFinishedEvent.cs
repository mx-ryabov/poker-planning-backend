using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Events;

public class VotingProcessFinishedEvent : IDomainEvent
{
    public VotingResult Result { get; }

    public VotingProcessFinishedEvent(VotingResult result)
    {
        Result = result;
    }
}
