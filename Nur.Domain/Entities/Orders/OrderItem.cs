using Nur.Domain.Commons;
using Nur.Domain.Entities.Products;

namespace Nur.Domain.Entities.Orders;

public class OrderItem : Auditable
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }

    public long OrderId { get; set; }
    public Order Order { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }

    public long CartItemId { get; set; }
    public CartItem CartItem { get; set; }
}
