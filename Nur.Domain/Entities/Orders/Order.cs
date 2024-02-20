using Nur.Domain.Enums;
using Nur.Domain.Commons;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Payments;
using Nur.Domain.Entities.Addresses;
using Nur.Domain.Entities.Suppliers;

namespace Nur.Domain.Entities.Orders;

public class Order : Auditable
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal? DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }

    public long AddressId { get; set; }
    public Address Address { get; set; }

    public long SupplierId { get; set; }
    public Supplier Supplier { get; set; }

    public long PaymentId { get; set; }
    public Payment Payment { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
