using BanNoiThat.Application.Service.PaymentMethod.PayVnService.Model;
using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.Service.PaymentMethod.PayVnService
{
    public interface IVnPayService
    {
        //Thực hiện việc trả về thanh toán vn pay
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
