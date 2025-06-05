using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;
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

        public async Task<OrderInfoModel> CreatePayment(string email, OrderInfoRequest orderInfo)
        {
            try
            {
                double totalPrice = 0;
                var orderEntity = new Order();
                var userEntity = await _uow.UserRepository.GetAsync(user => user.Email == email);
                var cartEntity = await _uow.CartRepository.GetCartByIdUser(userEntity.Id);

                orderEntity.Id = Guid.NewGuid().ToString();
                orderEntity.User_Id = userEntity.Id;
                orderEntity.OrderPaidTime = DateTime.Now;
                orderEntity.PaymentMethod = orderInfo.PaymentMethod;
                orderEntity.PaymentStatus = StaticDefine.Status_Payment_Pending;
                orderEntity.OrderStatus = StaticDefine.Status_Order_Pending;
                string address = orderInfo.ShippingAddress.Replace('-', ' ');
                orderEntity.ShippingAddress = $"{orderInfo.Province}-{orderInfo.District}-{orderInfo.Ward}-{address}";
                orderEntity.PhoneNumber = orderInfo.PhoneNumber;
                orderEntity.UserNameOrder = orderInfo.FullName; //*

                foreach (var cartItem in cartEntity.CartItems)
                {
                    cartItem.ProductItem.Quantity -= cartItem.Quantity;
                    await _uow.CartRepository.DeleteCartItem(cartEntity.Id, cartItem.Id);

                    if (cartItem.ProductItem.Quantity < 0)
                    {
                        //Đẩy ra là hết sản phẩm
                        throw new ArgumentException();
                    }

                    totalPrice = cartItem.Quantity * cartItem.ProductItem.SalePrice;
                    orderEntity.OrderItems.Add(new OrderItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Order_Id = orderEntity.Id,
                        NameItem = cartItem.ProductItem.Product.Name,
                        ImageItemUrl = cartItem.ProductItem.ImageUrl,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.ProductItem.SalePrice,
                        ProductItem_Id = cartItem.ProductItem_Id,
                    });
                }

                orderEntity.TotalPrice = totalPrice;
               
                await _uow.OrderRepository.CreateAsync(orderEntity);
                await _uow.SaveChangeAsync();

                return new OrderInfoModel()
                {
                    OrderId = orderEntity.Id,
                    FullName = orderInfo.FullName,
                    ShippingAddress = $"{orderEntity.ShippingAddress}",
                    PhoneNumber = orderInfo.PhoneNumber,
                    TotalPrice = totalPrice,
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
        }

        public async Task<OrderInfoModel> CreatePaymentOneProductItem(string email, OrderInfoRequest orderInfo)
        {
            try
            {
                double totalPrice = 0;
                var orderEntity = new Order();
                var userEntity = await _uow.UserRepository.GetAsync(user => user.Email == email);

                orderEntity.Id = Guid.NewGuid().ToString();
                orderEntity.User_Id = userEntity.Id;
                orderEntity.OrderPaidTime = DateTime.Now;
                orderEntity.PaymentMethod = orderInfo.PaymentMethod;
                orderEntity.PaymentStatus = StaticDefine.Status_Payment_Pending;
                orderEntity.OrderStatus = StaticDefine.Status_Order_Pending;
                string address = orderInfo.ShippingAddress.Replace('-', ' ');
                orderEntity.ShippingAddress = $"{orderInfo.Province}-{orderInfo.District}-{orderInfo.Ward}-{address}";
                orderEntity.PhoneNumber = orderInfo.PhoneNumber;
                orderEntity.UserNameOrder = orderInfo.FullName; //*

                var entityProductItem = await _uow.ProductRepository.GetProductItemByIdAsync(orderInfo.ProductItemId);

                entityProductItem.Quantity -= entityProductItem.Quantity;

                if (entityProductItem.Quantity < 0)
                {
                    //Đẩy ra là hết sản phẩm
                    throw new ArgumentException();
                }

                totalPrice = orderInfo.Quantity * entityProductItem.SalePrice;

                orderEntity.OrderItems.Add(new OrderItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Order_Id = orderEntity.Id,
                    NameItem = entityProductItem.Product.Name,
                    ImageItemUrl = entityProductItem.ImageUrl,
                    Quantity = orderInfo.Quantity,
                    Price = entityProductItem.SalePrice,
                    ProductItem_Id = entityProductItem.Id,
                });

                orderEntity.TotalPrice = totalPrice;

                await _uow.OrderRepository.CreateAsync(orderEntity);
                await _uow.SaveChangeAsync();

                return new OrderInfoModel()
                {
                    OrderId = orderEntity.Id,
                    FullName = orderInfo.FullName,
                    ShippingAddress = $"{orderEntity.ShippingAddress}",
                    PhoneNumber = orderInfo.PhoneNumber,
                    TotalPrice = totalPrice,
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
        }
    }
}
