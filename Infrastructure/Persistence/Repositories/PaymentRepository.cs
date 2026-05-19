using Application.Abstractions.Repositories;
using Domain.Entities.Payments;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _context.Payments.AddAsync(payment, cancellationToken);
        }

        public async Task<Payment?> GetByAuthorityAsync(string authority, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.Authority == authority);
        }

        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
        }
    }
}
