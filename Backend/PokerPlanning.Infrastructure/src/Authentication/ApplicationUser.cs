using Microsoft.AspNetCore.Identity;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Infrastructure.src.Authentication;

public class ApplicationUser : IdentityUser<Guid>
{
    public IdentityRole<Guid> Role { get; set; } = null!;

    public User User { get; set; } = null!;
    public ApplicationUser() { }
    public ApplicationUser(string userName, User user, IdentityRole<Guid> role) : base(userName)
    {
        Role = role;
        User = user;
    }

    /*public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? DisplayName { get; set; } = null!;

    public static ApplicationUser? CreateApplicationUser<TUser>(TUser? user) where TUser : User
    {
        if (user is GuestUser guest)
        {
            return CreateApplicationUser(guest);
        }
        else if (user is MemberUser member)
        {
            return CreateApplicationUser(member);
        }
        else if (user is null)
        {
            return null;
        }
        throw new NotImplementedException($"{user.GetType().Name} class isn't implemented within Application User");
    }

    private static ApplicationUser? CreateApplicationUser(GuestUser user)
    {
        return new ApplicationUser()
        {
            DisplayName = user.DisplayName
        };
    }

    private static ApplicationUser? CreateApplicationUser(MemberUser user)
    {
        return new ApplicationUser()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public User? GetDomainUser()
    {
        return Role.Name switch
        {
            "Guest" => GetDomainGuestUser(),
            "Member" => GetDomainMemberUser(),
            _ => throw new NotImplementedException($"GetDomainUser doesn't have implementation for converting it to the domain user with {Role} role"),
        };
    }

    private GuestUser GetDomainGuestUser()
    {
        return GuestUser.Create(
                    id: Id,
                    displayName: DisplayName ?? "Not specified"
                );
    }

    private MemberUser GetDomainMemberUser()
    {
        return MemberUser.Create(
            id: Id,
            email: Email ?? "Not specified",
            firstName: FirstName ?? "Not specified",
            lastName: LastName ?? "Not specified"
        );
    }*/
}
