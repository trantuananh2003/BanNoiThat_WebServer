using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IRepository
{
    public interface IRolesRepository : IRepository<Role>
    {
        void AddRoleClaim(string roleId, string claimType, string claimValue);
        Task DeleteRoleClaim(string roleClaimId);
    }
}
