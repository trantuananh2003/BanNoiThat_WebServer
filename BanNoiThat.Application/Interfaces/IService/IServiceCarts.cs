using BanNoiThat.Application.DTOs;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceCarts 
    {
        Task<CartResponse> GetCartByUserId(string UserId);
        Task UpdateQuantityItemCartByUserId(string UserId, CartItemRequest cartItemRequest);
    }
}
