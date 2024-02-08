using Nur.Application.UseCases.Attachments.DTOs;

namespace Nur.Application.UseCases.Vehicles.DTOs;

public class VehicleDTO
{
    public long Id { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarNumber { get; set; }
    public string Color { get; set; }
    public AttachmentDTO Attachment { get; set; }
}
