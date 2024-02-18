using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.ProductCategories.DTOs;

public class ProductCategoryDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ProductDTO> Products { get; set; }
}
