using Nur.APIService.Models.Addresses;

namespace Nur.APIService.Interfaces;

public interface IAddressService
{
    public Task<AddressDTO> AddAsync(AddressCreationDTO dto, CancellationToken cancellationToken);
    public Task<AddressDTO> UpdateAsync(AddressDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<AddressDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<AddressDTO>> GetAllAsync(CancellationToken cancellationToken);
}
