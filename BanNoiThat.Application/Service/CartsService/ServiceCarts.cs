using AutoMapper;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.CartsService
{
    public class ServiceCarts(IUnitOfWork uow, IMapper mapper) : IServiceCarts
    {
        public IUnitOfWork _uow = uow;
        public IMapper _mapper = mapper;

        public async Task<CartResponse> GetCartByUserId(string UserId)
        {
            var cartEntity = await _uow.CartRepository.GetCartByIdUser(UserId);
            //*Nếu giỏ hàng chưa tồn tại
            if (cartEntity is null)
            {
                await CreateCart(cartEntity, UserId);
                await _uow.SaveChangeAsync();
            }

            var cartResponse = _mapper.Map<CartResponse>(cartEntity);
            
            return cartResponse;
        }

        //Thêm hoặc giảm chưa áp dụng chỉnh việc nhập số lượng cố định
        public async Task UpdateQuantityItemCartByUserId(string UserId, CartItemRequest cartItemRequest)
        {
            if (cartItemRequest.IsAddManual)
            {
                cartItemRequest.Quantity /= Math.Abs(cartItemRequest.Quantity);
            }

            var cartEntity = await _uow.CartRepository.GetAsync(x => x.User_Id == UserId, includeProperties: "CartItems");
            _uow.CartRepository.AttachEntity(cartEntity);

            //*Nếu giỏ hàng chưa tồn tại
            if (cartEntity is null)
            {
                await CreateCart(cartEntity, UserId);
            }

            var productItem = await _uow.ProductRepository.GetProductItemByIdAsync(cartItemRequest.ProductItem_Id);
            var existingCartItem = cartEntity.CartItems.FirstOrDefault(x => x.ProductItem_Id == productItem.Id);

            //* Kiểm tra quantity muốn thêm vào có vượt quá
            var countQuantity = ((existingCartItem is not null) ? existingCartItem.Quantity : 0) + cartItemRequest.Quantity;

            //Kiểm tra số lượng hiện tại "Lớn hơn" ?  Báo lỗi vượt quá : Thao tác update cartEntity item
            if (countQuantity > productItem.Quantity)
            {
                //Trả về thông báo số lượng vượt quá
                throw new Exception()
                {
                };
            }

            //*Thao tác update cartEntity item
            //"Có cartEntity item" ? "Sử dụng lại cartEntity item" : tạo ra 1 cái cartEntity item mới
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItemRequest.Quantity;
                if(existingCartItem.Quantity <= 0)
                {
                    cartEntity.CartItems.Remove(existingCartItem);
                }
            }
            else
            {
                cartEntity.CartItems.Add(new CartItem() {
                    Id = Guid.NewGuid().ToString(),
                    ProductItem_Id = productItem.Id,
                    Quantity = productItem.Quantity,
                    Cart_Id = cartEntity.Id,
                });
            }

            await _uow.SaveChangeAsync();
        }

        //Tạo cart entity trong database
        private async Task CreateCart(Cart cart, string UserId)
        {
            cart = new Cart();
            cart.User_Id = UserId;
            cart.Id = Guid.NewGuid().ToString();
            await _uow.CartRepository.CreateAsync(cart);
        }
    }
}
