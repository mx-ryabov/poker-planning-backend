using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Results;

namespace PokerPlanning.Application.src.VotingSystemNS.Queries.VotingSystems;

public class VotinSystemsQueryHandler :
    IRequestHandler<VotingSystemsQuery, IEnumerable<VotingSystemResult>>
{
    private IVotingSystemRepository _votingSystemRepository;

    public VotinSystemsQueryHandler(IVotingSystemRepository votingSystemRepository)
    {
        _votingSystemRepository = votingSystemRepository;
    }
    public async Task<IEnumerable<VotingSystemResult>> Handle(VotingSystemsQuery request, CancellationToken cancellationToken)
    {
        var votingSystems = await _votingSystemRepository.Get();
        return votingSystems.Select(vs => new VotingSystemResult(
            Id: vs.Id,
            Name: vs.Name,
            Creator: null,
            Votes: vs.Votes.OrderBy(vsVote => vsVote.Order).Select(vsVote => new VotingSystemVoteResult(
                Id: vsVote.Id,
                Value: vsVote.Value,
                Order: vsVote.Order,
                Suit: vsVote.Suit,
                VotingSystemId: vsVote.VotingSystemId
            ))
        ));
    }
}