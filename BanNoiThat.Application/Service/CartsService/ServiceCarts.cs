using AutoMapper;
using BanNoiThat.Application.DTOs.CartDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.CartsService
{
    public class ServiceCarts(IUnitOfWork uow, IMapper mapper) : IServiceCarts
    {
        public IUnitOfWork _uow = uow;
        public IMapper _mapper = mapper;

        public async Task<CartResponse> GetCartByUserId(string userId)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            var cartEntity = await _uow.CartRepository.GetCartByIdUser(userEntity.Id);
            //*Nếu giỏ hàng chưa tồn tại
            if (cartEntity is null)
            {
                cartEntity = await CreateCart(cartEntity, userEntity.Id);
                await _uow.SaveChangeAsync();
            }

            var cartResponse = _mapper.Map<CartResponse>(cartEntity);
            
            return cartResponse;
        }

        //Thêm hoặc giảm chưa áp dụng chỉnh việc nhập số lượng cố định
        public async Task UpdateQuantityItemCartByUserId(string userId, CartItemRequest cartItemRequest)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            var cartEntity = await _uow.CartRepository.GetAsync(x => x.User_Id == userEntity.Id, includeProperties: "CartItems");

            //*Nếu giỏ hàng chưa tồn tại
            if (cartEntity is null)
            {
                cartEntity = await CreateCart(cartEntity, userEntity.Id);
            }
            else
            {
                _uow.CartRepository.AttachEntity(cartEntity);
            }

            var productItem = await _uow.ProductRepository.GetProductItemByIdAsync(cartItemRequest.ProductItem_Id);
            var existingCartItem = cartEntity.CartItems is not null ? cartEntity.CartItems.FirstOrDefault(x => x.ProductItem_Id == productItem.Id) : null;
            int countQuantity = 0;

            //* Kiểm tra quantity muốn thêm vào có vượt quá
            if (cartItemRequest.IsAddManual) {
                countQuantity = ((existingCartItem is not null) ? existingCartItem.Quantity : 0) + cartItemRequest.Quantity;
            }
            else
            {
                countQuantity = cartItemRequest.Quantity;
            }

            //Kiểm tra số lượng hiện tại "Lớn hơn" ?  Báo lỗi vượt quá : Thao tác update cartEntity item
            if (countQuantity > productItem.Quantity)
            {
                throw new Exception("Vượt quá số lượng");
            }

            //*Thao tác update cartEntity item
            //"Có cartEntity item" ? "Sử dụng lại cartEntity item" : tạo ra 1 cái cartEntity item mới
            if (existingCartItem != null)
            {
                if (cartItemRequest.IsAddManual)
                {
                    existingCartItem.Quantity += cartItemRequest.Quantity;
                }
                else
                {
                    existingCartItem.Quantity = countQuantity;
                }

                if (existingCartItem.Quantity <= 0)
                {
                    cartEntity.CartItems.Remove(existingCartItem);
                }
            }
            else
            {
                cartEntity.CartItems.Add(new CartItem() {
                    Id = Guid.NewGuid().ToString(),
                    ProductItem_Id = productItem.Id,
                    Quantity = cartItemRequest.Quantity,
                    Cart_Id = cartEntity.Id,
                });
            }

            await _uow.SaveChangeAsync();
        }  

        //Tạo cart entity trong database
        public async Task<Cart> CreateCart(Cart cart, string UserId)
        {
            cart = new Cart();
            cart.User_Id = UserId;
            cart.Id = Guid.NewGuid().ToString();
            cart.CartItems = new List<CartItem>();
            await _uow.CartRepository.CreateAsync(cart);

            return cart;
        }

        public async Task DeleteCartItem(string cartId,string cartItemId)
        {
            await _uow.CartRepository.DeleteCartItem(cartId, cartItemId);
            await _uow.SaveChangeAsync();
        }
    }
}
