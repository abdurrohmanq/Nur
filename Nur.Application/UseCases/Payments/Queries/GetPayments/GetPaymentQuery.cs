using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Payments;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Payments.DTOs;

namespace Nur.Application.UseCases.Payments.Queries.GetPayments;

public class GetPaymentQuery : IRequest<PaymentDTO>
{
    public GetPaymentQuery(long paymentId) { Id = paymentId; }
    public long Id { get; set; }
}

public class GetPaymentQueryHandler(IMapper mapper,
    IRepository<Payment> repository) : IRequestHandler<GetPaymentQuery, PaymentDTO>
{
    public async Task<PaymentDTO> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        => mapper.Map<PaymentDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This payment is not found with id: {request.Id}"));
}
