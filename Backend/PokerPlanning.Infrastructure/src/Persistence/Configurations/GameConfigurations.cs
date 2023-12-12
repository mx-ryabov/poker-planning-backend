using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.GameAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class GameConfigurations : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Name).IsRequired().HasColumnType("varchar(255)");
        builder.Property(g => g.Link).IsRequired().HasColumnType("varchar(255)");
        builder.OwnsOne(g => g.Settings);
        builder.OwnsOne(
            g => g.VotingProcess,
            vpb =>
            {
                vpb.HasOne(vp => vp.Ticket)
                    .WithOne()
                    .IsRequired(false);
                vpb.Property(vp => vp.IsActive).IsRequired().HasDefaultValue(false);
            });
        builder.HasOne(g => g.VotingSystem)
            .WithMany()
            .HasForeignKey("VotingSystemId")
            .IsRequired();
        builder.HasMany(g => g.Participants)
            .WithOne(p => p.Game)
            .IsRequired();
        builder.HasMany(g => g.Tickets)
            .WithOne(t => t.Game)
            .HasForeignKey(tb => tb.Id);
        builder.HasMany(g => g.VotingResults)
            .WithOne(vr => vr.Game)
            .HasForeignKey(vrB => vrB.Id);
        builder.ToTable("Games");
    }
}
