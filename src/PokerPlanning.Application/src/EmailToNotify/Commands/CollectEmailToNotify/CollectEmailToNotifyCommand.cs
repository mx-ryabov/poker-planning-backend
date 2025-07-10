using MediatR;

namespace PokerPlanning.Application.src.EmailToNotifyFeature.Commands;

public record CollectEmailToNotiyCommand(
    string Email
) : IRequest;

