using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Infrastructure.src.Authentication;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(au => au.User)
            .WithOne()
            .HasForeignKey<User>(u => u.Id)
            .IsRequired();
    }
}
