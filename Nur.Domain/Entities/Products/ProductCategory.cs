using Nur.Domain.Commons;

namespace Nur.Domain.Entities.Products;

public class ProductCategory : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }
}
