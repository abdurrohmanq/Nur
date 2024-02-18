using Nur.Application.UseCases.Attachments.DTOs;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Domain.Enums;

namespace Nur.Application.UseCases.Products.DTOs;

public class ProductDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public double Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }

    public ProductCategoryDTO Category { get; set; }
    public AttachmentDTO Attachment { get; set; }
}
