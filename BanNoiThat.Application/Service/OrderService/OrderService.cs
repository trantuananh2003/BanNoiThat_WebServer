
using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.OrderDtos;
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

        public async Task OrderUpdateStatus(string orderId, string orderStatus=null, string paymentStatus=null)
        {
            var entity = await _uow.OrderRepository.GetAsync(x => x.Id == orderId, tracked: true);

            if(!string.IsNullOrEmpty(orderStatus))
            {
                entity.OrderStatus = orderStatus;
            }
            if(!string.IsNullOrEmpty(paymentStatus))
            {
                entity.PaymentStatus = paymentStatus;
            }

            if (paymentStatus == StaticDefine.Status_Order_Cancelled)
            {
                await ReturnQuantityProduct(orderId);
            }

            await _uow.SaveChangeAsync();
        }

        private async Task ReturnQuantityProduct(string orderId)
        {
            var order = await _uow.OrderRepository.GetOrderIncludeAsync(orderId);
            
            foreach(var orderItem in order.OrderItems)
            {
                orderItem.ProductItem.Quantity += orderItem.Quantity;
            }
        }
    }
}
