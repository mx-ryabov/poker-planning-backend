using Microsoft.AspNetCore.Identity;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;

namespace PokerPlanning.Infrastructure.src.Authentication;

public class ApplicationUserRole : IdentityRole<Guid>
{
    public ApplicationUserRole() { }
    public ApplicationUserRole(string role) : base(role) { }
    public static Guid GetRoleId(UserRole role)
    {
        switch (role)
        {
            case UserRole.Guest: 
                return Guid.Parse("91c050e7-0576-4961-8753-75a39473bcc0");
            case UserRole.Member:
                return Guid.Parse("6cbc5b47-1527-4259-b8d0-e6c0d0513c3b");
            case UserRole.Admin:
                return Guid.Parse("eca7a853-d704-4545-a31c-67a78e88d599");
            default: throw new Exception("This role is undefined");
        }
    }
}
