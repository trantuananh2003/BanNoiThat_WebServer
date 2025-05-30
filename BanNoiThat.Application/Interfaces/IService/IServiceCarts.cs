using BanNoiThat.Application.DTOs.CartDtos;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceCarts 
    {
        Task<CartResponse> GetCartByUserId(string userId);
        Task UpdateQuantityItemCartByUserId(string userId, CartItemRequest cartItemRequest);
        Task DeleteCartItem(string cartId, string cartItemId);
    }
}
