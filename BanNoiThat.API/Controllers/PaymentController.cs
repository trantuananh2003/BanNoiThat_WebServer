using Azure;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Service.MomoService.Momo;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult> CreatePaymentUrl([FromQuery] string userId,[FromForm]OrderInfoRequest model) 
        {
            var orderModel =  await _paymentService.CreatePayment(userId, model);
            if(model.PaymentMethod == "COD")
            {
                return Created();
            }
            else if (model.PaymentMethod == "Online" && orderModel != null)
            {
                var resultMomo = await _momoService.CreatePaymentMomoAsync(orderModel);
                return RedirectPermanent(resultMomo.PayUrl);
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
