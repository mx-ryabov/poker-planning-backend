using MediatR;
using PokerPlanning.Application.src.Services.Authentication;

namespace PokerPlanning.Application.src.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<AuthenticationResult>;