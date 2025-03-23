using BanNoiThat.Application.Service.PayVnService.Model;
using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.Service.PayVnService
{
    public interface IVnPayService
    {
        //Thực hiện việc trả về thanh toán vn pay
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
