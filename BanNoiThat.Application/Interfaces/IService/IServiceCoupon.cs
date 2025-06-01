using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceCoupon
    {
        Task<ResultCheckCoupon> CheckCouponInOrder(string couponCode, Cart cart);
    }
}
