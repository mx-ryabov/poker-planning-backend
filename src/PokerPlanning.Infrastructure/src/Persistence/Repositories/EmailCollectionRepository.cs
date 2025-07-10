using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Repositories;

public class EmailCollectionRepository : IEmailCollectionRepository
{
    private readonly PokerPlanningDbContext _dbContext;

    public EmailCollectionRepository(PokerPlanningDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CollectEmail(EmailToNotify email, CancellationToken cancellationToken)
    {
        await _dbContext.EmailsToNotify.AddAsync(email, cancellationToken);
    }
}