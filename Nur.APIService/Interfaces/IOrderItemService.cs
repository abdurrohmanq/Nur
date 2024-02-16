using Nur.APIService.Models.OrderItems;

namespace Nur.APIService.Interfaces;

public interface IOrderItemService
{
    public Task<OrderItemResultDTO> AddAsync(OrderItemCreationDTO dto, CancellationToken cancellationToken);
    public Task<OrderItemResultDTO> UpdateAsync(OrderItemUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<OrderItemResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderItemResultDTO>> GetByOrderIdAsync(long userId, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderItemResultDTO>> GetByProductIdAsync(long supplierId, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderItemResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
