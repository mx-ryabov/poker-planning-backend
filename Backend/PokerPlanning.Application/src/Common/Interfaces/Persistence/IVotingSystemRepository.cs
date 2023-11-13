using PokerPlanning.Domain.src.Models.VotingSystemAggregate;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IVotingSystemRepository
{
    IEnumerable<VotingSystem> Get();
}