using Nur.APIService.Models.Products;

namespace Nur.APIService.Interfaces;

public interface IProductService
{
    public Task<ProductResultDTO> AddAsync(ProductCreationDTO dto, CancellationToken cancellationToken);
    public Task<ProductResultDTO> UpdateAsync(ProductUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<ProductResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<ProductResultDTO>> GetByCategoryIdAsync(long categoryId, CancellationToken cancellationToken);
    public Task<IEnumerable<ProductResultDTO>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
    public Task<IEnumerable<ProductResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
