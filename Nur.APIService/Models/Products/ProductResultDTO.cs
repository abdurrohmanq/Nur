using Nur.APIService.Models.Attachments;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.ProductCategories;

namespace Nur.APIService.Models.Products;

public class ProductResultDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }
    public ProductCategoryDTO Category { get; set; }
    public AttachmentResultDTO Attachment { get; set; }
}
