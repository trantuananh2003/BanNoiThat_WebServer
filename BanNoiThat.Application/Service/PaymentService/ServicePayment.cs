using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.MomoService.Momo;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.PaymentService
{
    public class ServicePayment : IServicePayment
    {
        private readonly IMomoService _momoService; //Tai sao private readonly lại khong hoat dong vs Class
        private IUnitOfWork _uow;

        public ServicePayment(IMomoService momoService, IUnitOfWork uow)
        {
            _momoService = momoService;
            _uow = uow;
        }

        public async Task<OrderInfoModel> CreatePayment(string userId, OrderInfoRequest orderInfo)
        {
            double totalPrice = 0;
            var orderEntity = new Order();
            var cartEntity = await _uow.CartRepository.GetCartByIdUser(userId);

            orderEntity.Id = Guid.NewGuid().ToString();
            orderEntity.User_Id = userId;
            orderEntity.OrderPaidTime = DateTime.Now;

            orderEntity.PaymentMethod = orderInfo.PaymentMethod;
            
            orderEntity.PaymentStatus = "Pending";
            orderEntity.OrderStatus = "Pending";

            orderEntity.ShippingAddress = orderInfo.ShippingAddress;
            orderEntity.PhoneNumber = orderInfo.PhoneNumber;
            orderEntity.PaymentIntentId = orderInfo.FullName; //*

            foreach (var cartItem in cartEntity.CartItems)
            {
                cartItem.ProductItem.Quantity -= cartItem.Quantity;

                if (cartItem.ProductItem.Quantity < 0)
                {
                    //Đẩy ra là hết sản phẩm
                    throw new ArgumentException();
                }

                totalPrice = cartItem.Quantity * cartItem.ProductItem.Price;
                orderEntity.OrderItems.Add(new OrderItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Order_Id = orderEntity.Id,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.ProductItem.Price,
                    ProductItem_Id = cartItem.ProductItem_Id,
                });
            }

            orderEntity.TotalPrice = totalPrice;

            await _uow.OrderRepository.CreateAsync(orderEntity);
            await _uow.SaveChangeAsync();

            return new OrderInfoModel() {
                OrderId = orderEntity.Id,
                FullName = orderInfo.FullName,
                ShippingAddress = orderEntity.ShippingAddress,
                PhoneNumber = orderInfo.PhoneNumber,
                TotalPrice = totalPrice,
            };
        }
    }
}
