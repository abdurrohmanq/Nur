using Nur.APIService.Models.CartItems;
using Nur.APIService.Models.Orders;
using Nur.APIService.Models.Products;

namespace Nur.APIService.Models.OrderItems;

public class OrderItemResultDTO
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public OrderResultDTO Order { get; set; }
    public ProductResultDTO Product { get; set; }
}