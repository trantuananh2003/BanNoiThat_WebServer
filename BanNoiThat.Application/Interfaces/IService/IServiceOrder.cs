using BanNoiThat.Application.DTOs;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceOrder
    {
        Task<Order> GetDetailOrderById(string id, string userId);
        Task<List<OrderResponse>> GetListOrder(string userId, string orderStatus);
    }
}
