using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;

namespace PokerPlanning.Infrastructure.src.Persistence.Configurations;

public class EmailToNotifyConfigurations : IEntityTypeConfiguration<EmailToNotify>
{
    public void Configure(EntityTypeBuilder<EmailToNotify> builder)
    {
        builder.Property(vr => vr.Email)
            .IsRequired();
        builder.Property(vr => vr.CreatedAt)
            .IsRequired();
        builder.ToTable("EmailsToNotify");
    }
}
