using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using BanNoiThat.Infrastructure.SqlServer.Migrations;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task AddFavoriteProduct(FavoriteProducts entity)
        {
            await _db.FavoriteProducts.AddAsync(entity);
        }
    }
}
