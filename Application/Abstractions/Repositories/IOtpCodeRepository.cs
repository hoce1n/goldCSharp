using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Application.Abstractions.Repositories
{
    public interface IOtpCodeRepository
    {
        Task AddAsync(OtpCode otp);

        Task<OtpCode?> GetLatestAsync(PhoneNumber phoneNumber);
            
        Task<OtpCode?> GetActiveCodeAsync(
        PhoneNumber phone,
        CancellationToken cancellationToken);

        Task<IEnumerable<OtpCode>> GetActiveCodesAsync(
            PhoneNumber phone, 
            CancellationToken cancellationToken);
        Task<int> CountRecentAsync(PhoneNumber phoneNumber, DateTime since);

        Task UpdateAsync(OtpCode otp);
    }
}