using Nur.APIService.Models.Vehicles;

namespace Nur.APIService.Interfaces;

public interface IVehicleService
{
    public Task<VehicleResultDTO> AddAsync(VehicleCreationDTO dto, CancellationToken cancellationToken);
    public Task<VehicleResultDTO> UpdateAsync(VehicleUpdateDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<VehicleResultDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<VehicleResultDTO>> GetAllAsync(CancellationToken cancellationToken);
}
