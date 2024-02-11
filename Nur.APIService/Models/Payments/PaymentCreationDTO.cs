using Nur.APIService.Models.Enums;

namespace Nur.APIService.Models.Payments;

public class PaymentCreationDTO
{
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}
