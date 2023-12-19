
using Microsoft.AspNetCore.Identity;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.Infrastructure.src.Authentication;
using PokerPlanning.Infrastructure.src.Exceptions.ApplicationUser;

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

    public async Task CreateGuest(GuestUser user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);

        var role = new IdentityRole<Guid>("Guest");
        var applicationUser = new ApplicationUser($"guest{Guid.NewGuid()}", user, role);
        var result = await _userManager.CreateAsync(applicationUser, "111111");

        if (!result.Succeeded)
        {
            var errors = String.Join("; ", result.Errors.Select(e => e.Description));
            throw new ApplicationUserCreationException(errors);
        }
    }
}