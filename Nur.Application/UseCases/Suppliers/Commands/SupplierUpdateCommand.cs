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

public class SupplierUpdateCommand : IRequest<SupplierDTO>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public long VehicleId { get; set; }
    public IFormFile Attachment { get; set; }
}

public class SupplierUpdateCommandHandler(IMapper mapper,
    IRepository<Supplier> repository,
    IRepository<Vehicle> vehicleRepository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentCreateHandler,
    IRequestHandler<AttachmentRemoveCommand, bool> attachmentRemoveHandler)
  : IRequestHandler<SupplierUpdateCommand, SupplierDTO>
{
    public async Task<SupplierDTO> Handle(SupplierUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(s => s.Id.Equals(request.Id))
           ?? throw new NotFoundException($"This supplier not found with Id: {request.Id}");

        if (entity.VehicleId != request.Id)
        {
            var vehicle = await vehicleRepository.SelectAsync(c => c.Id.Equals(request.VehicleId))
                ?? throw new NotFoundException($"This vehicle is not found with id: {request.VehicleId}");
            entity.VehicleId = request.VehicleId;
            entity.Vehicle = vehicle;
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Phone = request.Phone;
        entity.DateOfBirth = request.DateOfBirth.AddHours(TimeConstant.UTC);

        if (request.Attachment is not null)
        {
            var attachment = await attachmentCreateHandler.Handle(new AttachmentCreateCommand
            { FormFile = request.Attachment }, cancellationToken);

            await attachmentRemoveHandler.Handle(new AttachmentRemoveCommand 
            { Attachment = entity.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<SupplierDTO>(entity);
    }
}
