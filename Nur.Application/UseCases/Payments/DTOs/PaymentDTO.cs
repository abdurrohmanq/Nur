using Nur.Domain.Enums;

namespace Nur.Application.UseCases.Payments.DTOs;

public class PaymentDTO
{
    public long Id { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}
