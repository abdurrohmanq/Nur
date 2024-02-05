using Nur.Domain.Commons;
using Nur.Domain.Entities.Attachments;

namespace Nur.Domain.Entities.Suppliers;

public class Supplier : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public long AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}