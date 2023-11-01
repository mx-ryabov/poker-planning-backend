using MediatR;
using PokerPlanning.Application.src.Authentication.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Services.Authentication;

namespace PokerPlanning.Application.src.Authentication.Queries.Login;

public class LoginQueryHandler :
    IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.FromResult(0);
        var user = _userRepository.GetUserByEmail(query.Email);

        if (user is null || user?.Password != query.Password)
        {
            throw new WrongPasswordOrEmailException();
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            token);
    }
}