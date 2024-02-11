using Nur.Domain.Commons;
using Nur.Domain.Entities.Users;

namespace Nur.Domain.Entities.Carts;

public class Cart : Auditable
{
    public decimal TotalPrice { get; set; }
    public long? UserId { get; set; }
    public User User { get; set; }

    public ICollection<CartItem> CartItems { get; set; }
}
