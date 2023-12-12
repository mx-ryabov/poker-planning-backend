using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Game.Results;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;

namespace PokerPlanning.Application.src.Game.Commands.Create;

public class CreateGameCommandHandler :
    IRequestHandler<CreateGameCommand, GameResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public CreateGameCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<GameResult> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        /*
        1. Create guest by name
        2. Create game with the created guest
        */
        var guest = await _userRepository.CreateGuest(GuestUser.Create(request.CreatorName), cancellationToken);



        throw new NotImplementedException();
    }
}
