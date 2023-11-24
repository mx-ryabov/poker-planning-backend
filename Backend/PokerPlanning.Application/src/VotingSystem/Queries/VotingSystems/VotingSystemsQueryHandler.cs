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
            Creator: vs.Creator is null ? null : new UserResult(
                Id: vs.Creator.Id,
                FirstName: vs.Creator.FirstName,
                LastName: vs.Creator.LastName,
                Email: vs.Creator.Email
            ),
            Votes: vs.Votes.Select(vsVote => new VotingSystemVoteResult(
                Id: vsVote.Id,
                Value: vsVote.Value,
                Order: vsVote.Order,
                Suit: vsVote.Suit,
                VotingSystemId: vsVote.VotingSystemId
            ))
        ));
    }
}