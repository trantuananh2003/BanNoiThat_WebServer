
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

        public async Task<List<OrderResponse>> GetListOrderForClient(string userId, string orderStatus)
        {
            var listOrder = await _uow.OrderRepository.GetAllAsync(x => x.User_Id == userId && x.OrderStatus == orderStatus, includeProperties: "OrderItems");
            var listOrderResponse = _mapper.Map<List<OrderResponse>>(listOrder);

            return listOrderResponse;
        }

        public async Task<List<OrderResponse>> GetListOrderForAdmin(string userId, string orderStatus)
        {
            var listOrder = await _uow.OrderRepository.GetAllAsync(x => x.OrderStatus == orderStatus, includeProperties: "OrderItems");
            var listOrderResponse = _mapper.Map<List<OrderResponse>>(listOrder);

            return listOrderResponse;
        }

        public async Task OrderUpdateStatus(string idUser, string orderId, string orderStatus)
        {
            var user = await _uow.UserRepository.GetAsync(x => x.Id == idUser);
            var entity = await _uow.OrderRepository.GetAsync(x => x.Id == orderId && x.User_Id == user.Id);
            _uow.OrderRepository.AttachEntity(entity);

            entity.OrderStatus = orderStatus;
            await _uow.SaveChangeAsync();
        }

    }
}
