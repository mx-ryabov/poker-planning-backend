using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class TicketConfigurations : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnType("varchar(255)");
        builder.Property(t => t.Description)
            .IsRequired()
            .HasDefaultValue("")
            .HasColumnType("text");
        builder.Property(t => t.Link)
            .IsRequired(false)
            .HasColumnType("varchar(500)");
        builder.Property(t => t.Type)
            .IsRequired(false)
            .HasConversion<string>()
            .HasColumnType("varchar(100)");
        builder.Property(t => t.Identifier)
            .IsRequired(false)
            .HasColumnType("varchar(50)");
        builder.Property(t => t.Estimation)
            .IsRequired(false)
            .HasColumnType("varchar(50)");
        builder.HasOne(t => t.Game)
            .WithMany(g => g.Tickets)
            .HasForeignKey(t => t.GameId)
            .IsRequired(false);
        builder.ToTable("Tickets");
    }
}
