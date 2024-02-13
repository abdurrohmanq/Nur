using Nur.APIService.Models.Users;

namespace Nur.APIService.Interfaces;

public interface IUserService
{
    public Task<UserDTO> AddAsync(UserCreationDTO dto, CancellationToken cancellationToken);
    public Task<UserDTO> UpdateAsync(UserDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<UserDTO> GetAsync(long telegramId, CancellationToken cancellationToken);
    public Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
}
