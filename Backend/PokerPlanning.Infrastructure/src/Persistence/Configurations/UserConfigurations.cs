using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(u => u.LastName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(u => u.Email).IsRequired().HasColumnType("varchar(255)");
        builder.ToTable("Users");
    }
}
