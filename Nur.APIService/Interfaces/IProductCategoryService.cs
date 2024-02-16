using Nur.APIService.Models.ProductCategories;

namespace Nur.APIService.Interfaces;

public interface IProductCategoryService
{
    public Task<ProductCategoryDTO> AddAsync(ProductCategoryCreationDTO dto, CancellationToken cancellationToken);
    public Task<ProductCategoryDTO> UpdateAsync(ProductCategoryDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<ProductCategoryDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<ProductCategoryDTO>> GetAllAsync(CancellationToken cancellationToken);
}
