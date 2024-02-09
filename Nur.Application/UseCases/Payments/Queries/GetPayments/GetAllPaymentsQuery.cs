using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Payments.DTOs;

namespace Nur.Application.UseCases.Payments.Queries.GetPayments;

public class GetAllPaymentsQuery : IRequest<IEnumerable<PaymentDTO>>
{ }

public class GetAllPaymentsQueryHandler(IMapper mapper,
    IRepository<Payment> repository) : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDTO>>
{
    public async Task<IEnumerable<PaymentDTO>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await repository.SelectAll().ToListAsync();
        var mappedPayments = mapper.Map<IEnumerable<PaymentDTO>>(payments);
        return mappedPayments;
    }
}
