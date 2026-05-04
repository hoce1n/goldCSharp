using Domain.Entities.Identity;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task UpdateAsync(RefreshToken token);
    Task<RefreshToken?> GetByHashAsync(string hash);
}