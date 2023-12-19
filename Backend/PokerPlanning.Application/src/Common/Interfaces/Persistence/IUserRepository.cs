using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task CreateGuest(GuestUser user, CancellationToken cancellationToken);
}