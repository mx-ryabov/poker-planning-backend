using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.DoVote;

public class DoVoteCommandHandler : IRequestHandler<DoVoteCommand, DoVoteResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoVoteCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DoVoteResult> Handle(DoVoteCommand command, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(
            gameId: command.GameId,
            userId: command.UserId,
            cancellationToken
        ) ?? throw new NotFoundException("Participant");

        var result = participant.DoVote(command.VoteId);
        if (!result.Success)
        {
            throw new DoVoteException(String.Join("; ", result.Errors));
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return new DoVoteResult(
            ParticipantId: participant.Id,
            VoteId: command.VoteId
        );
    }
}
