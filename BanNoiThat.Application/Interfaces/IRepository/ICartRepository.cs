using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByIdUser(string UserId);
        Task DeleteCartItem(string idCart, string idCartItem);
    }
}
