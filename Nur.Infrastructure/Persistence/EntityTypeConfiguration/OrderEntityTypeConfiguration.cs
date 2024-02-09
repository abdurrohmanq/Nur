using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nur.Infrastructure.Persistence.EntityTypeConfiguration;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.StartAt)
               .HasColumnType("timestamp");
        
        builder.Property(o => o.EndAt)
               .HasColumnType("timestamp");
    }
}
