using Nur.APIService.Models.Carts;

namespace Nur.APIService.Interfaces;

public interface ICartService
{
    public Task<CartDTO> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
}
