using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
        Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
