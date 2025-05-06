using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddFavoriteProduct(FavoriteProducts entity);
    }
}
