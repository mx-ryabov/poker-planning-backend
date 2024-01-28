using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.Property(p => p.DisplayName).HasColumnType("varchar(255)").IsRequired();
        builder.HasOne(p => p.Vote)
            .WithMany()
            .HasForeignKey(p => p.VoteId)
            .IsRequired(false);
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .IsRequired(false);
        builder.HasOne(p => p.Game)
            .WithMany(g => g.Participants)
            .HasForeignKey(p => p.GameId)
            .IsRequired();
        builder.Property(p => p.Role)
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .IsRequired();
        builder.ToTable("Participants");
    }
}
