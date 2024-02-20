using Nur.APIService.Models.Enums;

namespace Nur.APIService.Models.Orders;

public class OrderUpdateDTO
{
    public long Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal? DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public long SupplierId { get; set; }
    public long PaymentId { get; set; }
}
