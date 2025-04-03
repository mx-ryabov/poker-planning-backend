using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Commands.Create;

public class CreateGameCommandHandler :
    IRequestHandler<CreateGameCommand, CreateGameResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGameCommandHandler(
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

    public async Task<CreateGameResult> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var guest = GuestUser.Create(request.CreatorName);
        var master = Participant.Create(guest.DisplayName, ParticipantRole.Master, guest);
        var game = Game.Create(
            name: request.Name,
            link: Guid.NewGuid().ToString(),
            settings: new GameSettings() { IsAutoRevealCards = request.Settings.IsAutoRevealCards },
            votingSystemId: request.VotingSystemId,
            master: master
        );

        await _unitOfWork.BeginAsync(cancellationToken);
        try
        {
            await _userRepository.CreateGuest(guest, cancellationToken);
            await _gameRepository.Create(game, cancellationToken);
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

        return new CreateGameResult(
            Id: game.Id,
            Name: game.Name,
            Link: game.Link,
            Settings: new GameSettingsResult(game.Settings.IsAutoRevealCards),
            Participants: game.Participants.Select(
                p => new GameParticipantResult(
                    p.Id,
                    p.DisplayName,
                    p.Online,
                    p.Role,
                    p.User?.Id,
                    null
                )
            ).ToList(),
            MasterToken: token
        );
    }
}
