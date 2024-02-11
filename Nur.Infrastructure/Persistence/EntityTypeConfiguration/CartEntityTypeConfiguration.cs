using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nur.Domain.Entities.Carts;

namespace Nur.Infrastructure.Persistence.EntityTypeConfiguration;

public class CartEntityTypeConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasOne(u => u.User)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);
    }
}
