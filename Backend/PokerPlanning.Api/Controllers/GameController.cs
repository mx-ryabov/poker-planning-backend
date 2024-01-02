using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PokerPlanning.Api.Hubs;
using PokerPlanning.Application.src.GameFeature.Commands.ChangeVotingProcess;
using PokerPlanning.Application.src.GameFeature.Commands.Create;
using PokerPlanning.Application.src.GameFeature.Commands.DoVote;
using PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;
using PokerPlanning.Application.src.GameFeature.Queries.GetGame;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Contracts.src.Game;
using PokerPlanning.Contracts.src.GameHub;

namespace PokerPlanning.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IHubContext<GameHub> _hubContext;

    public GameController(ISender sender, IHubContext<GameHub> hubContext)
    {
        _sender = sender;
        _hubContext = hubContext;
    }

    [HttpGet("{gameId}")]
    [Authorize]
    public async Task<ActionResult> Get([FromRoute] Guid gameId)
    {
        GetGameResult gameResult = await _sender.Send(new GetGameQuery(gameId));
        return Ok(gameResult);
    }

    [HttpPost("")]
    public async Task<ActionResult> Create([FromBody] CreateGameRequest req)
    {
        var command = new CreateGameCommand(
            Name: req.Name,
            VotingSystemId: req.VotingSystemId,
            Settings: new CreateGameSettings(
                IsAutoRevealCards: req.IsAutoRevealCards
            ),
            CreatorName: req.CreatorName
        );

        CreateGameResult gameResult = await _sender.Send(command);

        var response = new CreateGameResponse(
            Id: gameResult.Id,
            Name: gameResult.Name,
            Link: gameResult.Link,
            Settings: new CreateGameSettingResponse(
                IsAutoRevealCards: gameResult.Settings.IsAutoRevealCards
            ),
            MasterToken: gameResult.MasterToken
        );
        return Ok(response);
    }

    [HttpPost("{gameId}/join-as-guest")]
    public async Task<ActionResult> JoinAsGuest([FromBody] JoinAsGuestRequest req, [FromRoute] Guid gameId)
    {
        JoinAsGuestGameResult joinAsGuestResult = await _sender.Send(new JoinAsGuestGameCommand(gameId, req.DisplayName));
        return Ok(joinAsGuestResult);
    }

    [HttpPut("{gameId}/voting-process")]
    [Authorize]
    public async Task<ActionResult> Voting([FromRoute] Guid gameId, [FromBody] ChangeVotingProcessRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        await _sender.Send(new ChangeVotingProcessCommand(
            GameId: gameId,
            UserId: new Guid(userId),
            IsActive: request.IsActive,
            TicketId: request.TicketId
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.VotingProcessChanged,
                new VotingProcessChangedResponse(
                    request.IsActive,
                    request.TicketId
                )
            );

        return Ok();
    }

    [HttpPut("{gameId}/vote")]
    [Authorize]
    public async Task<ActionResult> DoVote([FromRoute] Guid gameId, [FromBody] DoVoteRequest? request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        var result = await _sender.Send(new DoVoteCommand(
            GameId: gameId,
            UserId: new Guid(userId),
            VoteId: request?.VoteId
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.ParticipantVoted,
                new ParticipantVotedResponse(
                    ParticipantId: result.ParticipantId,
                    VoteId: result.VoteId
                )
            );

        return Ok();
    }
}
