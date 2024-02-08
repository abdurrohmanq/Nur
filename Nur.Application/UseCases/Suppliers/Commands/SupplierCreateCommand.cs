using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Attachments;
using Nur.Application.Commons.Constants;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Suppliers.DTOs;
using Nur.Application.UseCases.Attachments.Commands;

namespace Nur.Application.UseCases.Suppliers.Commands;

public class SupplierCreateCommand : IRequest<SupplierDTO>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public long VehicleId { get; set; }
    public IFormFile Attachment { get; set; }
}

public class SupplierCreateCommandHandler(IMapper mapper,
       IRepository<Supplier> supplierRepository,
       IRepository<Vehicle> vehicleRepository,
       IRequestHandler<AttachmentCreateCommand, Attachment> attachmentHandler) : IRequestHandler<SupplierCreateCommand, SupplierDTO>
{
    public async Task<SupplierDTO> Handle(SupplierCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await supplierRepository.SelectAsync(s => s.Phone.Equals(request.Phone));
        if (entity is not null)
            throw new AlreadyExistException($"This supplier is not found with phone: {request.Phone}");

        var existVehicle = await vehicleRepository.SelectAsync(v => v.Id.Equals(request.VehicleId))
            ?? throw new NotFoundException($"This vehicle is not found with id: {request.VehicleId}");

        entity = new Supplier
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth.AddHours(TimeConstant.UTC),
            VehicleId = request.VehicleId,
            Vehicle = existVehicle
        };

        if (request.Attachment is not null)
        {
            var attachment = await attachmentHandler.Handle(new AttachmentCreateCommand { FormFile = request.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        await supplierRepository.InsertAsync(entity);
        await supplierRepository.SaveAsync();

        return mapper.Map<SupplierDTO>(entity);
    }
}
