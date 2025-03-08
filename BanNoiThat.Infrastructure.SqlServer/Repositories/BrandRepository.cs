using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandsRepository
    {
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
