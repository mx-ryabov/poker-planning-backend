using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class GameConfigurations : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");
        builder.Property(g => g.Link)
            .IsRequired()
            .HasColumnType("varchar(255)");
        builder.Property(g => g.TicketsSequenceNumber)
            .IsRequired()
            .HasDefaultValue(1)
            .HasColumnType("integer");
        builder.OwnsOne(
            g => g.Settings,
            settings =>
            {
                settings.Property(s => s.IsAutoRevealCards).HasDefaultValue(false);
                settings.Property(s => s.AutoRevealPeriod).HasDefaultValue(120);
            });
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.OwnsOne(
            g => g.VotingProcess,
            vpb =>
            {
                vpb.HasOne(vp => vp.Ticket)
                    .WithOne()
                    .IsRequired(false);
                vpb.Property(vp => vp.Status).IsRequired().HasDefaultValue(VotingStatus.Inactive);
                vpb.Property(vp => vp.StartTime)
                    .HasConversion(dateTimeConverter)
                    .HasDefaultValue(null);
            });
        builder.HasOne(g => g.VotingSystem)
            .WithMany()
            .HasForeignKey(g => g.VotingSystemId)
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
