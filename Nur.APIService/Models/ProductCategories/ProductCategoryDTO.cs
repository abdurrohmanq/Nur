using Nur.APIService.Models.Products;

namespace Nur.APIService.Models.ProductCategories;

public class ProductCategoryDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ProductResultDTO> Products { get; set; }

}
