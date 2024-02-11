using Nur.APIService.Models.Attachments;

namespace Nur.APIService.Models.Vehicles;

public class VehicleResultDTO
{
    public long Id { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarNumber { get; set; }
    public string Color { get; set; }
    public AttachmentResultDTO Attachment { get; set; }
}
