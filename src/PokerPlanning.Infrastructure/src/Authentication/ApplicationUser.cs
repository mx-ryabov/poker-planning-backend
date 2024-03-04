using Microsoft.AspNetCore.Identity;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Infrastructure.src.Authentication;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUserRole Role { get; set; } = null!;
    public Guid RoleId {  get; set; }

    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public ApplicationUser() { }
    public ApplicationUser(string userName, User user, Guid roleId) : base(userName)
    {
        Id = Guid.NewGuid();
        RoleId = roleId;
        User = user;
    }
}
