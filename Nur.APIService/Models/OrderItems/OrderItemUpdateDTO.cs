namespace Nur.APIService.Models.OrderItems;

public class OrderItemUpdateDTO
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public long CartItemId { get; set; }
}
