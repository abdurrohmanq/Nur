using Nur.Application.UseCases.Attachments.DTOs;

namespace Nur.Application.UseCases.Suppliers.DTOs;

public class SupplierDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public AttachmentDTO Attachment { get; set; }
}
