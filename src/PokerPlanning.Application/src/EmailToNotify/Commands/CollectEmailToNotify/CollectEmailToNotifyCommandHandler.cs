using MediatR;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;

namespace PokerPlanning.Application.src.EmailToNotifyFeature.Commands;

public class CollectEmailToNotiyCommandHandler :
    IRequestHandler<CollectEmailToNotiyCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailCollectionRepository _emailCollectionRepository;

    public CollectEmailToNotiyCommandHandler(
        IUnitOfWork unitOfWork,
        IEmailCollectionRepository emailCollectionRepository
    )
    {
        _unitOfWork = unitOfWork;
        _emailCollectionRepository = emailCollectionRepository;
    }

    public async Task Handle(CollectEmailToNotiyCommand request, CancellationToken cancellationToken)
    {
        var email = EmailToNotify.Create(request.Email);

        await _unitOfWork.BeginAsync(cancellationToken);
        try
        {
            await _emailCollectionRepository.CollectEmail(email, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
