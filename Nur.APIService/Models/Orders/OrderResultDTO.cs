using Nur.APIService.Models.Addresses;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.Payments;
using Nur.APIService.Models.Suppliers;
using Nur.APIService.Models.Users;

namespace Nur.APIService.Models.Orders;

public class OrderResultDTO
{
    public long Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public UserDTO User { get; set; }
    public AddressDTO Address { get; set; }
    public SupplierResultDTO Supplier { get; set; }
    public PaymentDTO Payment { get; set; }
}
