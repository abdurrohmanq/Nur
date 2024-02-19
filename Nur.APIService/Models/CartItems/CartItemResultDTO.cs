using Nur.APIService.Models.Carts;
using Nur.APIService.Models.Products;

namespace Nur.APIService.Models.CartItems;

public class CartItemResultDTO
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public CartDTO Cart { get; set; }
    public ProductResultDTO Product { get; set; }
}
