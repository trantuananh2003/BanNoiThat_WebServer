using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;
using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomoAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
