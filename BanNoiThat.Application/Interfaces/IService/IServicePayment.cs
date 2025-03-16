using BanNoiThat.Application.Service.MomoService.Momo;
using BanNoiThat.Application.Service.PaymentService;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServicePayment
    {
        Task<OrderInfoModel> CreatePayment(string email, OrderInfoRequest orderInfo);
    }
}
