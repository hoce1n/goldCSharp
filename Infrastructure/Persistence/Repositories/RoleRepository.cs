//using Application.Abstractions.Repositories;
//using Domain.Entities.Identity;
//using Infrastructure.Persistence.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Persistence.Repositories
//{
//    public class RoleRepository : IRoleRepository
//    {
//        private readonly AppDbContext _context;

//        public RoleRepository(AppDbContext context) 
//        {
//            _context = context;
//        }

//        public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
//        {
//            return await _context.Roles
//                .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
//        }

//        public async Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
//        {
//            return await _context.Roles
//                .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
//        }

//        public async Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
//        {
//            return await _context.Roles.ToListAsync(cancellationToken);
//        }
//    }
//}
