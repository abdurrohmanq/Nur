using Nur.APIService.Models.Attachments;
using Nur.APIService.Models.Vehicles;

namespace Nur.APIService.Models.Suppliers;

public class SupplierResultDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public VehicleResultDTO Vehicle { get; set; }
    public AttachmentResultDTO Attachment { get; set; }
}
