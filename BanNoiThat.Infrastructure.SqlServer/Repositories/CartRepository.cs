using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private ApplicationDbContext _db;
        

        public CartRepository(ApplicationDbContext db) : base(db) 
        { 
            _db = db;
        }

        public async Task<Cart> GetCartByIdUser(string UserId)
        {
            var cartEntity = await _db.Carts.Where(cart => cart.User_Id == UserId)
                .Include(x => x.CartItems).ThenInclude(x => x.ProductItem).ThenInclude(x => x.Product).FirstOrDefaultAsync();

            return cartEntity;
        }

        public async Task DeleteCartItem(string cartId, string cartItemId)
        {
            var entity = await _db.CartItems.Where(x => x.Id == cartItemId).FirstOrDefaultAsync();

            _db.CartItems.Remove(entity);
        }
    }
}
