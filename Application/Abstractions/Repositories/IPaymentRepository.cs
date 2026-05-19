using Domain.Entities.Payments;

namespace Application.Abstractions.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Payment?> GetByAuthorityAsync(string authority, CancellationToken cancellationToken);
        Task AddAsync(Payment payment, CancellationToken cancellationToken);
        void Update(Payment payment);
    }
}
