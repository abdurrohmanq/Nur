namespace Nur.APIService.Models.OrderItems;

public class OrderItemCreationDTO
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}
