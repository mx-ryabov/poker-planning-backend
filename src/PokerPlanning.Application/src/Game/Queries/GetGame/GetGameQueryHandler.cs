using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Application.src.Results;
using PokerPlanning.Domain.src.Models.GameAggregate;

namespace PokerPlanning.Application.src.GameFeature.Queries.GetGame;

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GetGameResult>
{
    private IGameRepository _gameRepository;

    public GetGameQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GetGameResult> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.Get(request.GameId, cancellationToken) ?? throw new NotFoundException("Game");
        return MapToResult(game);
    }

    private GetGameResult MapToResult(Game game)
    {
        return new GetGameResult(
            Id: game.Id,
            Name: game.Name,
            Link: game.Link,
            Settings: new GameSettingsResult(
                IsAutoRevealCards: game.Settings.IsAutoRevealCards
            ),
            VotingProcess: new GameVotingProcessResult(
                Status: game.VotingProcess.Status,
                TicketId: game.VotingProcess.TicketId
            ),
            VotingSystem: new VotingSystemResult(
                Id: game.VotingSystem.Id,
                Name: game.VotingSystem.Name,
                Creator: null,
                Votes: game.VotingSystem.Votes.Select(v => new VotingSystemVoteResult(
                    Id: v.Id,
                    Value: v.Value,
                    Order: v.Order,
                    Suit: v.Suit,
                    VotingSystemId: v.VotingSystemId
                ))
            ),
            Participants: game.Participants.Select(p => new GameParticipantResult(
                Id: p.Id,
                DisplayName: p.DisplayName,
                Online: p.Online,
                Role: p.Role,
                UserId: p.UserId,
                Vote: p.Vote is null ? null : new GameVoteResult(
                    Id: p.Vote.Id,
                    Value: p.Vote.Value,
                    Suit: p.Vote.Suit
                )
            )),
            Tickets: game.Tickets.Select(t => new GameTicketResult(
                Id: t.Id,
                Title: t.Title,
                Description: t.Description,
                Link: t.Link,
                Type: t.Type,
                Identifier: t.Identifier,
                Estimation: t.Estimation
            )),
            VotingResults: game.VotingResults.Select(vr => new GameVotingResultResult(
                Id: vr.Id,
                TicketId: vr.TicketId,
                CreatedAt: vr.CreatedAt,
                Votes: vr.Votes.Select(vote => new GameVotingResultVoteResult(
                    Id: vote.Id,
                    VoteId: vote.VoteId,
                    Vote: vote.Vote is null ? null : new GameVoteResult(
                        Id: vote.Vote.Id,
                        Value: vote.Vote.Value,
                        Suit: vote.Vote.Suit
                    ),
                    ParticipantId: vote.ParticipantId
                ))
            ))
        );
    }
}
