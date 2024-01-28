using Microsoft.AspNetCore.Identity;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Infrastructure.src.Authentication;

public class ApplicationUser : IdentityUser<Guid>
{
    public IdentityRole<Guid> Role { get; set; } = null!;

    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public ApplicationUser() { }
    public ApplicationUser(string userName, User user, IdentityRole<Guid> role) : base(userName)
    {
        Role = role;
        User = user;
    }
}
