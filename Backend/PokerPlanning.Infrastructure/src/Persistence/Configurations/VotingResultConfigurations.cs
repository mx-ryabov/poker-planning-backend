using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class VotingResultConfigurations : IEntityTypeConfiguration<VotingResult>
{
    public void Configure(EntityTypeBuilder<VotingResult> builder)
    {
        builder.HasOne(vr => vr.Game)
            .WithMany(g => g.VotingResults)
            .HasForeignKey("GameId")
            .IsRequired();
        builder.HasOne(vr => vr.Ticket)
            .WithMany()
            .HasForeignKey("TicketId")
            .IsRequired(false);
        builder.HasMany(vr => vr.Votes)
            .WithOne()
            .HasForeignKey("VotingResultId");
        builder.Property(vr => vr.CreatedAt)
            .IsRequired();
        builder.ToTable("VotingResults");
    }
}
