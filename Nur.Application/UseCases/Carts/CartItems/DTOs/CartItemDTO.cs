using Nur.Application.UseCases.Carts.DTOs;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.DTOs;

public class CartItemDTO
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public CartDTO Cart { get; set; }

    public ProductDTO Product { get; set; }
}
