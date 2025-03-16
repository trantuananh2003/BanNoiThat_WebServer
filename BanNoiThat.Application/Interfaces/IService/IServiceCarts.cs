using BanNoiThat.Application.DTOs;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceCarts 
    {
        Task<CartResponse> GetCartByUserEmail(string UserId);
        Task UpdateQuantityItemCartByUserId(string emailUser, CartItemRequest cartItemRequest);
        Task DeleteCartItem(string cartId, string cartItemId);
    }
}
