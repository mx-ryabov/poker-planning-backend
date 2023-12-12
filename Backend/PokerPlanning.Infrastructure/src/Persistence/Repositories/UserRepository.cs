using Microsoft.AspNetCore.Identity;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.Infrastructure.src.Authentication;

namespace PokerPlanning.Infrastructure.src.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PokerPlanningDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(PokerPlanningDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<User> CreateGuest(GuestUser user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var applicationUser = new ApplicationUser($"guest{Guid.NewGuid()}", user, new IdentityRole<Guid>("Guest"));
        var result = await _userManager.CreateAsync(applicationUser, "111");

        if (result.Succeeded)
        {

        }

        return user;
    }
}