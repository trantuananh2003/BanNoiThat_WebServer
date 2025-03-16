
using AutoMapper;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.OrderService
{
    public class OrderService : IServiceOrder
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Order> GetDetailOrderById(string id, string userId)
        {
            var orderEntity = await _uow.OrderRepository.GetAsync(x => x.Id == id && x.User_Id == userId);
            return orderEntity;
        }

        public async Task<List<OrderResponse>> GetListOrder(string userId, string orderStatus)
        {
            var listOrder = await _uow.OrderRepository.GetAllAsync(x => x.User_Id == userId && x.OrderStatus == orderStatus, includeProperties: "OrderItems");
            var listOrderResponse = _mapper.Map<List<OrderResponse>>(listOrder);

            return listOrderResponse;
        }

    }
}
