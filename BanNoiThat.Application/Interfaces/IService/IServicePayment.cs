using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServicePayment
    {
        Task<OrderInfoModel> CreatePayment(string email, OrderInfoRequest orderInfo, List<ResultCheckCoupon> listResultCoupon);
    }
}
