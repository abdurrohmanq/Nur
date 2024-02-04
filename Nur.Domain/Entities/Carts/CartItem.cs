using Nur.Domain.Commons;
using Nur.Domain.Entities.Carts;
using Nur.Domain.Entities.Products;

public class CartItem : Auditable
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long CartId { get; set; }
    public Cart Cart { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }
}
