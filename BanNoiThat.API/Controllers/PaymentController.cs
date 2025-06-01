using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;
using BanNoiThat.Application.Service.PaymentMethod.PayVnService;
using BanNoiThat.Domain.Entities;
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
        private IVnPayService _vnpayService;
        private IServiceOrder _orderService;
        private IServiceCoupon _couponService;
        private IUnitOfWork _uow;
        
        public PaymentController(IMomoService momoservice, IVnPayService vnpayService, IUnitOfWork uow,
            IServicePayment paymentService, IServiceOrder orderService, IServiceCoupon serviceCoupon)
        {
            _apiResponse = new ApiResponse();
            _momoService = momoservice;
            _paymentService = paymentService;
            _vnpayService = vnpayService;
            _orderService = orderService;
            _couponService = serviceCoupon;
            _uow = uow;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreatePaymentUrl([FromForm] OrderInfoRequest model) 
        {
            var emailUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;

            var cart = await _uow.CartRepository.GetCartByIdUser(userId);
            List<ResultCheckCoupon> resultCheckCoupons = new List<ResultCheckCoupon>();

            foreach (var couponCode in model.CouponCodes)
            {
                var result = await _couponService.CheckCouponInOrder(couponCode, cart);
                resultCheckCoupons.Add(result);
                if (!result.IsCanApply)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessages.Add("Không thể áp mã");
                    return BadRequest(_apiResponse);
                }
            }

            var orderModel =  await _paymentService.CreatePayment(emailUser, model, resultCheckCoupons);

            if(model.PaymentMethod == "cod")
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                await _orderService.OrderUpdateStatus(orderModel.OrderId, StaticDefine.Status_Order_Processing, StaticDefine.Status_Payment_Pending);

                return Ok(_apiResponse);
            }
            else if (model.PaymentMethod == "momo" && orderModel != null)
            {
                var resultMomo = await _momoService.CreatePaymentMomoAsync(orderModel);
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = resultMomo.PayUrl;
                return Ok(_apiResponse);
            }
            else if(model.PaymentMethod == "vnpay" && orderModel != null)
            {
                var modelPaymentVNPAY = new Application.Service.PaymentMethod.PayVnService.Model.PaymentInformationModel()
                {
                    OrderType = "other",
                    Amount = (int)orderModel.TotalPrice,
                    OrderDescription = orderModel.OrderInformation,
                    Name = orderModel.FullName,
                    OrderId = orderModel.OrderId
                };

                var url =  _vnpayService.CreatePaymentUrl(modelPaymentVNPAY, HttpContext);
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = url;
                return Ok(_apiResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        //Ket qua thong tin giao dich VN pay
        [HttpGet("redirect")]
        public async Task<ActionResult<ApiResponse>> PaymentCallbackVnpay()
        {
            var response = _vnpayService.PaymentExecute(Request.Query);

            if (response.VnPayResponseCode == "00")
            {
                await _orderService.OrderUpdateStatus(response.OrderId, orderStatus: StaticDefine.Status_Order_Processing, paymentStatus: StaticDefine.Status_Payment_Paid);
                Response.Redirect("http://localhost:3005/payment-successful");
                return null;
            }
            else if (response.VnPayResponseCode == "24")
            {
                await _orderService.OrderUpdateStatus(response.OrderId, orderStatus: StaticDefine.Status_Order_Cancelled, paymentStatus: StaticDefine.Status_Payment_Failed);
            }

            return Ok(response);
        }

        //Ket qua thong tin giao dich momo
        [HttpGet("momo/payment-call-back")]
        public async Task<ActionResult> PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;

            return Ok(response);
        }

    }
}
