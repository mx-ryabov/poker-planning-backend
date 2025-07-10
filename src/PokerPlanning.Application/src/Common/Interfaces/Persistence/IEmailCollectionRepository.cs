using PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IEmailCollectionRepository
{
    Task CollectEmail(EmailToNotify email, CancellationToken cancellationToken);
}