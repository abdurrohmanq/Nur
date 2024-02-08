using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nur.Infrastructure.Persistence.EntityTypeConfiguration;

public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(d => d.DateOfBirth)
               .HasColumnType("timestamp");
    }
}
