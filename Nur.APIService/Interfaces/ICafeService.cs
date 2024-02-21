using Nur.APIService.Models.Cafes;

namespace Nur.APIService.Interfaces;

public interface ICafeService
{
    public Task<CafeDTO> AddAsync(CafeCreationDTO dto, CancellationToken cancellationToken);
    public Task<CafeDTO> UpdateAsync(CafeDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<IEnumerable<CafeDTO>> GetAsync(CancellationToken cancellationToken);
}
