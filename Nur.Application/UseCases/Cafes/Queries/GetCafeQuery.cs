using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Cafes.DTOs;
using Nur.Domain.Entities.Cafes;

namespace Nur.Application.UseCases.Cafes.Queries;

public class GetCafeQuery : IRequest<IEnumerable<CafeDTO>>
{ }

public class GetCafeQueryHandler(IMapper mapper,
    IRepository<Cafe> repository) : IRequestHandler<GetCafeQuery, IEnumerable<CafeDTO>>
{
    public async Task<IEnumerable<CafeDTO>> Handle(GetCafeQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<CafeDTO>>(await repository.SelectAll().ToListAsync());
}