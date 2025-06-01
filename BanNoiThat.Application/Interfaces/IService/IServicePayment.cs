using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;
using BanNoiThat.Application.Service.PaymentService;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServicePayment
    {
        Task<OrderInfoModel> CreatePayment(string email, OrderInfoRequest orderInfo, List<ResultCheckCoupon> listResultCoupon);
    }
}
