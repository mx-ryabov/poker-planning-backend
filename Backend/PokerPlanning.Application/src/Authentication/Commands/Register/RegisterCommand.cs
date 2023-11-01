using MediatR;
using PokerPlanning.Application.src.Services.Authentication;

namespace PokerPlanning.Application.src.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<AuthenticationResult>;