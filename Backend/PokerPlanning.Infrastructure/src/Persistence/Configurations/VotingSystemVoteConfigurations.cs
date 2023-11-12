using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class VotingSystemVoteConfigurations : IEntityTypeConfiguration<VotingSystemVote>
{
    public void Configure(EntityTypeBuilder<VotingSystemVote> builder)
    {
        builder.Property(vs => vs.Value).IsRequired().HasColumnType("varchar(50)");
        builder.Property(vs => vs.Order).IsRequired().HasColumnType("decimal");
        builder.Property(vs => vs.Suit).IsRequired().HasColumnType("varchar(5)");
    }
}
