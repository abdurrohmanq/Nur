namespace Nur.APIService.Models.CartItems;

public class CartItemCreationDTO
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
}