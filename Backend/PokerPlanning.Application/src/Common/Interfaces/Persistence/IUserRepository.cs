using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Application.src.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserById(Guid id);
    User? GetUserByEmail(string email);
    void Add(User user);
}