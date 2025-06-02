using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PokerPlanning.Api.Hubs;
using PokerPlanning.Application.src.GameFeature.Commands.StartVoting;
using PokerPlanning.Application.src.GameFeature.Commands.Create;
using PokerPlanning.Application.src.GameFeature.Commands.DoVote;
using PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;
using PokerPlanning.Application.src.GameFeature.Queries.GetGame;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Contracts.src.Game;
using PokerPlanning.Contracts.src.GameHub;
using PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;
using PokerPlanning.Application.src.GameFeature.Queries.GetParticipantById;
using PokerPlanning.Application.src.GameFeature.Commands.AddTicket;
using PokerPlanning.Application.src.GameFeature.Commands.DeleteTicket;
using PokerPlanning.Application.src.GameFeature.Commands.RevealCards;
using PokerPlanning.Application.src.GameFeature.Commands.UpdateTicket;
using PokerPlanning.Application.src.GameFeature.Commands.UpdateGameSettings;
using PokerPlanning.Domain.src.Common.DTO;

namespace PokerPlanning.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IHubContext<GameHub> _hubContext;

    public GameController(
        ISender sender,
        IHubContext<GameHub> hubContext)
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
    public async Task<ActionResult> Create(CreateGameRequest req)
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
                IsAutoRevealCards: gameResult.Settings.IsAutoRevealCards,
                AutoRevealPeriod: gameResult.Settings.AutoRevealPeriod
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

    [HttpPut("{gameId}/start-voting")]
    [Authorize]
    public async Task<ActionResult> StartVoting([FromRoute] Guid gameId, [FromBody] StartVotingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        var result = await _sender.Send(new StartVotingCommand(
            GameId: gameId,
            UserId: new Guid(userId),
            TicketId: request.TicketId,
            OnTimeIsUp: () =>
            {
                _hubContext.Clients
                    .Group(gameId.ToString())
                    .SendAsync(
                        GameHubMethods.CardsRevealed
                    );
            }
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.VotingStarted,
                new VotingStartedResponse(
                    TicketId: request.TicketId,
                    StartTime: result.StartTime
                )
            );

        return Ok(result);
    }

    [HttpPut("{gameId}/reveal-cards")]
    [Authorize]
    public async Task<ActionResult> RevealCards([FromRoute] Guid gameId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        await _sender.Send(new RevealCardsCommand(
            GameId: gameId,
            UserId: new Guid(userId)
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.CardsRevealed
            );

        return Ok();
    }

    [HttpPut("{gameId}/finish-voting")]
    [Authorize]
    public async Task<ActionResult> FinishVoting([FromRoute] Guid gameId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        var result = await _sender.Send(new FinishVotingCommand(
            GameId: gameId,
            UserId: new Guid(userId)
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.VotingFinished,
                result
            );

        return Ok(result);
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

    [HttpGet("{gameId}/current-participant")]
    [Authorize]
    public async Task<ActionResult> GetCurrentParticipant([FromRoute] Guid gameId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var participantResult = await _sender.Send(new GetParticipantByGameAndUserIdQuery(
            GameId: gameId,
            UserId: Guid.Parse(userId)
        ));
        return Ok(participantResult);
    }

    [HttpPost("{gameId}/ticket")]
    [Authorize]
    public async Task<ActionResult> CreateTicketForGame([FromRoute] Guid gameId, [FromBody] AddTicketRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var ticketResult = await _sender.Send(new AddTicketCommand(
            Title: request.Title,
            Type: request.Type,
            GameId: gameId,
            UserId: Guid.Parse(userId)
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.TicketAdded,
                ticketResult
            );
        return Ok(ticketResult);
    }

    [HttpPut("{gameId}/ticket/{ticketId}")]
    [Authorize]
    public async Task<ActionResult> UpdateTicketForGame([FromRoute] Guid gameId, [FromRoute] Guid ticketId, [FromBody] UpdateTicketRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var ticketResult = await _sender.Send(new UpdateTicketCommand(
            TicketId: ticketId,
            GameId: gameId,
            UserId: Guid.Parse(userId),
            Data: request.Data
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.TicketUpdated,
                ticketResult
            );
        return Ok(ticketResult);
    }

    [HttpDelete("{gameId}/ticket/{ticketId}")]
    [Authorize]
    public async Task<ActionResult> DeleteTicketFromGame([FromRoute] Guid gameId, [FromRoute] Guid ticketId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var ticketResult = await _sender.Send(new DeleteTicketCommand(
            TicketId: ticketId,
            GameId: gameId,
            UserId: Guid.Parse(userId)
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.TicketDeleted,
                ticketId
            );
        return Ok(ticketResult);
    }

    [HttpPut("{gameId}/settings")]
    [Authorize]
    public async Task<ActionResult> UpdateGameSettings([FromRoute] Guid gameId, [FromBody] UpdateGameSettingsRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var updateSettingsResult = await _sender.Send(new UpdateGameSettingsCommand(
            GameId: gameId,
            UserId: Guid.Parse(userId),
            Data: new UpdateGameSettingsDTO(
                Name: request.Name,
                IsAutoRevealCards: request.IsAutoRevealCards,
                AutoRevealPeriod: request.AutoRevealPeriod,
                GameMasterId: request.GameMasterId
            )
        ));
        await _hubContext.Clients
            .Group(gameId.ToString())
            .SendAsync(
                GameHubMethods.SettingsUpdated,
                updateSettingsResult
            );

        foreach (var p in updateSettingsResult.UpdatedParticipants)
        {
            var participantUserId = p.UserId.ToString();
            if (participantUserId is not null)
            {
                await _hubContext.Clients
                    .User(participantUserId)
                    .SendAsync(
                        GameHubMethods.CurrentParticipantUpdated,
                        p
                    );
            }
        }

        return Ok(updateSettingsResult);
    }
}
