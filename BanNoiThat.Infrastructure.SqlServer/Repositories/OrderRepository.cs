using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class OrderRepository : Repository<Order>, IOrdersRepository
    {
        private ApplicationDbContext _db;
        private DbSet<Order> _dbSet;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            _dbSet = _db.Set<Order>();
        }

        public async Task<Order> GetOrderIncludeAsync(string orderId)
        {
            IQueryable<Order> query = _dbSet;
            query = query.Where(x => x.Id == orderId).Include(p => p.OrderItems).ThenInclude(p => p.ProductItem);

            var order = await query.FirstOrDefaultAsync();
            return order;
        }

        public async Task<OrderItem> GetOrderItemById(string orderItemId)
        {
            return await _db.OrderItems.Where(x => x.Id == orderItemId).AsTracking().FirstOrDefaultAsync();
        }
    }
}
