using Nur.APIService.Models.CartItems;

namespace Nur.APIService.Interfaces;

public interface ICartItemService
{
    public Task<CartItemResultDTO> AddAsync(CartItemCreationDTO dto, CancellationToken cancellationToken);
    public Task<CartItemResultDTO> UpdateAsync(CartItemUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<CartItemResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<bool> DeleteByProductNameAsync(string productName, CancellationToken cancellationToken);
    public Task<IEnumerable<CartItemResultDTO>> GetByCartIdAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<CartItemResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
