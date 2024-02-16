using Nur.APIService.Models.Orders;

namespace Nur.APIService.Interfaces;

public interface IOrderService
{
    public Task<OrderResultDTO> AddAsync(OrderCreationDTO dto, CancellationToken cancellationToken);
    public Task<OrderResultDTO> UpdateAsync(OrderUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<OrderResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderResultDTO>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderResultDTO>> GetBySupplierIdAsync(long supplierId, CancellationToken cancellationToken);
    public Task<IEnumerable<OrderResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
