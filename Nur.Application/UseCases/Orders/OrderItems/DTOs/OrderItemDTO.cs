using Nur.Application.UseCases.Orders.DTOs;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.DTOs;

public class OrderItemDTO
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }

    public OrderDTO Order { get; set; }
    public ProductDTO Product { get; set; }
    public CartItemDTO CartItem { get; set; }
}
