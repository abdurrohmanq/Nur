using Nur.APIService.Models.Payments;

namespace Nur.APIService.Interfaces;

public interface IPaymentService
{
    public Task<PaymentDTO> AddAsync(PaymentCreationDTO dto, CancellationToken cancellationToken);
    public Task<PaymentDTO> UpdateAsync(PaymentDTO dto, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<PaymentDTO> GetAsync(long Id, CancellationToken cancellationToken);
    public Task<IEnumerable<PaymentDTO>> GetAllAsync(CancellationToken cancellationToken);
}
