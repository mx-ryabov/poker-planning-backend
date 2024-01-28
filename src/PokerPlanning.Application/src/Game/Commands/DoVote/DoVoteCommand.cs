using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.DoVote;

public record DoVoteCommand(
    Guid GameId,
    Guid UserId,
    Guid? VoteId
) : IRequest<DoVoteResult>;
