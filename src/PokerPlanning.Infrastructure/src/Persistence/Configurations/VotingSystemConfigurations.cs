using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class VotingSystemConfigurations : IEntityTypeConfiguration<VotingSystem>
{
    public void Configure(EntityTypeBuilder<VotingSystem> builder)
    {
        builder.Property(vs => vs.Name).IsRequired().HasColumnType("varchar(255)");
        builder.HasOne(vs => vs.Creator)
            .WithMany()
            .HasForeignKey("CreatorId")
            .IsRequired(false);
        builder.HasMany(vs => vs.Votes)
            .WithOne()
            .HasForeignKey(vsVote => vsVote.VotingSystemId)
            .IsRequired();
        builder.ToTable("VotingSystems");
    }
}
