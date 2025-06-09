using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IRepository
{
    public interface IOrdersRepository : IRepository<Order>
    {
        Task<Order> GetOrderIncludeAsync(string orderId);
        Task<OrderItem> GetOrderItemById(string orderItemId);
    }
}
