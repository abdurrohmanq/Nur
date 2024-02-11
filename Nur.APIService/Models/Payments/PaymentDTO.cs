using Nur.APIService.Models.Enums;

namespace Nur.APIService.Models.Payments;

public class PaymentDTO
{
    public long Id { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}
