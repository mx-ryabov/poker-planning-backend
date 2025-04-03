using PokerPlanning.Application.src.Results;

namespace PokerPlanning.Application.src.GameFeature.Results;

public record GetGameResult(
    Guid Id,
    string Name,
    string Link,
    GameSettingsResult Settings,
    GameVotingProcessResult VotingProcess,
    VotingSystemResult VotingSystem,
    IEnumerable<GameParticipantResult> Participants,
    IEnumerable<GameTicketResult> Tickets,
    IEnumerable<GameVotingResultResult> VotingResults
);
