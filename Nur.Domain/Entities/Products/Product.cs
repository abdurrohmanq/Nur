using Nur.Domain.Commons;
using Nur.Domain.Enums;
using Nur.Domain.Entities.Attachments;

namespace Nur.Domain.Entities.Products;

public class Product : Auditable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public double Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }

    public long CategoryId { get; set; }
    public ProductCategory Category { get; set; }

    public long? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}
