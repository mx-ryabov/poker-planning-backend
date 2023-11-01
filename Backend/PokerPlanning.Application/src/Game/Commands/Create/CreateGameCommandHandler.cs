using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Game.Results;

namespace PokerPlanning.Application.src.Game.Commands.Create;

public class CreateGameCommandHandler :
    IRequestHandler<CreateGameCommand, GameResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public CreateGameCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public Task<GameResult> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
