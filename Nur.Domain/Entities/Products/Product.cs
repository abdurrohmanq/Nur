using Nur.Domain.Commons;
using Nur.Domain.Entities.Attachments;
using Nur.Domain.Enums;

namespace Nur.Domain.Entities.Products;

public class Product : Auditable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }

    public long CategoryId { get; set; }
    public ProductCategory Category { get; set; }

    public long? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}
