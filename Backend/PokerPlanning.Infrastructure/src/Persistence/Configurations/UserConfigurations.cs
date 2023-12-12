using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate.MemberUserAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.HasDiscriminator(u => u.Role)
            .HasValue<GuestUser>(UserRole.Guest)
            .HasValue<MemberUser>(UserRole.Member);
        builder.ToTable("Users");
    }
}

public class GuestConfigurations : IEntityTypeConfiguration<GuestUser>
{
    public void Configure(EntityTypeBuilder<GuestUser> builder)
    {
        ConfigureGuestsTable(builder);
    }

    private void ConfigureGuestsTable(EntityTypeBuilder<GuestUser> builder)
    {
        builder.Property(gu => gu.DisplayName)
            .IsRequired()
            .HasColumnType("varchar(255)");
    }
}

public class MemberConfigurations : IEntityTypeConfiguration<MemberUser>
{
    public void Configure(EntityTypeBuilder<MemberUser> builder)
    {
        ConfigureMembersTable(builder);
    }

    private void ConfigureMembersTable(EntityTypeBuilder<MemberUser> builder)
    {
        builder.Property(mu => mu.FirstName)
            .IsRequired()
            .HasColumnType("varchar(255)");
        builder.Property(mu => mu.LastName)
            .IsRequired()
            .HasColumnType("varchar(255)");
        builder.Property(mu => mu.Email)
            .IsRequired()
            .HasColumnType("varchar(255)");
    }
}