using MediatR;
using PokerPlanning.Application.src.Authentication.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Services.Authentication;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Application.src.Authentication.Commands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await Task.FromResult(0);
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            throw new UserAlreadyExistsException();
        }

        var user = User.Create(
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            password: command.Password
        );
        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            token);
    }
}