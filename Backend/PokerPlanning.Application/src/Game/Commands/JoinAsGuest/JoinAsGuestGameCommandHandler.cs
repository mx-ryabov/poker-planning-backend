using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;

namespace PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;

public class JoinAsGuestGameCommandHandler :
    IRequestHandler<JoinAsGuestGameCommand, JoinAsGuestGameResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JoinAsGuestGameCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<JoinAsGuestGameResult> Handle(JoinAsGuestGameCommand request, CancellationToken cancellationToken)
    {
        var guest = GuestUser.Create(request.DisplayName);

        await _unitOfWork.BeginAsync(cancellationToken);
        try
        {
            await _userRepository.CreateGuest(guest, cancellationToken);

            var participant = Participant.Create(guest.DisplayName, ParticipantRole.VotingMember, guest);
            await _gameRepository.CreateParticipant(participant, cancellationToken);

            var game = await _gameRepository.Get(request.gameId, cancellationToken);
            var result = game.AddParticipant(participant);
            if (!result.Success)
            {
                throw new AddParticipantException(String.Join("; ", result.Errors));
            }
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: consider moving the token generation to the controller level (probably becuase it shouldn't be in Application/Domain levels at all)
        var token = _jwtTokenGenerator.GenerateToken(guest.DisplayName, guest.Id, guest.Role.ToString());

        return new JoinAsGuestGameResult(token);
    }
}
