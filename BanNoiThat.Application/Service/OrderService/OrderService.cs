
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
        private ServiceShipping _serviceShipping;

        public OrderService(IUnitOfWork uow, IMapper mapper, ServiceShipping serviceShipping) {
            _uow = uow;
            _mapper = mapper;
            _serviceShipping = serviceShipping;
        }

        public async Task<Order> GetDetailOrderById(string id, string userId)
        {
            var orderEntity = await _uow.OrderRepository.GetAsync(x => x.Id == id);
            return orderEntity;
        }

        public async Task<List<OrderResponse>> GetListOrderForClient(string userId, string orderStatus)
        {
            var listOrder = await _uow.OrderRepository.GetAllAsync(
                x => x.User_Id == userId && x.OrderStatus == orderStatus,
                includeProperties: "OrderItems"
            );
            var sortedListOrder = listOrder.OrderByDescending(x => x.OrderPaidTime).ToList();
            var listOrderResponse = _mapper.Map<List<OrderResponse>>(sortedListOrder);

            return listOrderResponse;
        }

        public async Task<List<OrderResponse>> GetListOrderForAdmin(string orderStatus)
        {
            var listOrder = await _uow.OrderRepository.GetAllAsync(x => x.OrderStatus == orderStatus, includeProperties: "OrderItems");
            var listOrderResponse = _mapper.Map<List<OrderResponse>>(listOrder);

            //Gọi để kiểm tra trạng thái đơn hàng
            var listOrderShipping = await _uow.OrderRepository.GetAllAsync(x => x.OrderStatus == StaticDefine.Status_Order_Shipping, includeProperties: "OrderItems", isTracked: true);

            foreach (var order in listOrderShipping)
            {
                var statusGHN = await CheckStatusOrderGHN(order.Id);
                if(statusGHN.Data.status == "delivered")
                {
                    await OrderUpdateStatus(order.Id, orderStatus: StaticDefine.Status_Order_Done, paymentStatus: StaticDefine.Status_Payment_Paid);
                    await SetSoldQuantityProductItem(order.Id);
                }
                else if(statusGHN.Data.status == "return" || statusGHN.Data.status == "returned")
                {
                    await OrderUpdateStatus(order.Id, orderStatus: StaticDefine.Status_Order_Returned);
                }
                await _uow.SaveChangeAsync();
            }
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

        private async Task<OrderDetailGHNReponse> CheckStatusOrderGHN(string orderId)
        {
            var entityOrder = await _uow.OrderRepository.GetAsync(x => x.Id == orderId);
            var statusOrder = await _serviceShipping.GetStatusOrder("a85473ec-2e75-11f0-9b81-222185cb68c8", entityOrder.AddressCode);

            return statusOrder;
        }
    
        private async Task SetSoldQuantityProductItem(string orderItemId)
        {
            var entityOrder = await _uow.OrderRepository.GetOrderIncludeAsync(orderItemId);

            foreach(var orderItem in entityOrder.OrderItems)
            {
                orderItem.ProductItem.SoldQuantity += orderItem.Quantity;
            }

            await _uow.SaveChangeAsync();
        }
    }
}
