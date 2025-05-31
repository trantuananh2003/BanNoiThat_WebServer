using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class RoleRepository : Repository<Role>, IRolesRepository
    {
        private ApplicationDbContext _db;

        public RoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void AddRoleClaim(string roleId, string claimType, string claimValue)
        {
            _db.RoleClaims.Add(new RoleClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Role_Id = roleId,
                ClaimType = claimType,
                ClaimValue = claimValue
            });            
        }

        public async Task DeleteRoleClaim(string roleClaimId)
        {
            var entity = await _db.RoleClaims.FirstOrDefaultAsync(x => x.Id == roleClaimId);
            _db.RoleClaims.Remove(entity);
        }
    }
}
