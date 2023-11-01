using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PokerPlanningDbContext _dbContext;

    public UserRepository(PokerPlanningDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private static readonly List<User> _users = new();

    public void Add(User user)
    {
        _dbContext.Add(user);
        _dbContext.SaveChanges();
    }

    public User? GetUserByEmail(string email)
    {
        return _users.SingleOrDefault(u => u.Email == email);
    }

    public User? GetUserById(Guid id)
    {
        return _users.SingleOrDefault(u => u.Id == id);
    }
}