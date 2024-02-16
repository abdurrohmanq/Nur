using Nur.APIService.Models.Suppliers;

namespace Nur.APIService.Interfaces;

public interface ISupplierService
{
    public Task<SupplierResultDTO> AddAsync(SupplierCreationDTO dto, CancellationToken cancellationToken);
    public Task<SupplierResultDTO> UpdateAsync(SupplierUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<SupplierResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<SupplierResultDTO>> GetByVehicleAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<SupplierResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
