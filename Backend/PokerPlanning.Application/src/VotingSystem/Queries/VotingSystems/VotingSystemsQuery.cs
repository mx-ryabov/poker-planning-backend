using MediatR;
using PokerPlanning.Application.src.Results;

namespace PokerPlanning.Application.src.VotingSystemNS.Queries.VotingSystems;

public record VotingSystemsQuery() : IRequest<IEnumerable<VotingSystemResult>>;
