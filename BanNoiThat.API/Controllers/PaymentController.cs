using BanNoiThat.API.Model;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Service.MomoService.Momo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PaymentController : Controller
    {
        private readonly ApiResponse _apiResponse;
        private IMomoService _momoService;
        private IServicePayment _paymentService;
        
        public PaymentController(IMomoService momoservice, IServicePayment paymentService)
        {
            _apiResponse = new ApiResponse();
            _momoService = momoservice;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreatePaymentUrl([FromForm] OrderInfoRequest model) 
        {
            var emailUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;

            var orderModel =  await _paymentService.CreatePayment(emailUser, model);

            if(model.PaymentMethod == "cod")
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            else if (model.PaymentMethod == "momo" && orderModel != null)
            {
                var resultMomo = await _momoService.CreatePaymentMomoAsync(orderModel);
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = resultMomo.PayUrl;
                return Ok(_apiResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        //Ket qua thong tin giao dich
        [HttpGet("momo/payment-call-back")]
        public async Task<ActionResult> PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;

            return Ok(response);
        }
    }
}
