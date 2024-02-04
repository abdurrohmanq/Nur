using Nur.Domain.Commons;
using Nur.Domain.Enums;

namespace Nur.Domain.Entities.Payments;

public class Payment : Auditable
{
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}
