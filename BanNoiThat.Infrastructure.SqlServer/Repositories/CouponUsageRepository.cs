using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class CouponUsageRepository : Repository<CouponUsage>, ICouponUsageRepository
    {
        public CouponUsageRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
