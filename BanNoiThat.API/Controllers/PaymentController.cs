using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.DTOs.PaymentDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo;
using BanNoiThat.Application.Service.PaymentMethod.PayVnService;
using BanNoiThat.Application.Service.PaymentService;
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
        private readonly ServiceShipping _shippingService;
        private IUnitOfWork _uow;
        
        public PaymentController(IMomoService momoservice, IVnPayService vnpayService, IUnitOfWork uow,
            IServicePayment paymentService, IServiceOrder orderService, IServiceCoupon serviceCoupon, ServiceShipping shippingService)
        {
            _apiResponse = new ApiResponse();
            _momoService = momoservice;
            _paymentService = paymentService;
            _vnpayService = vnpayService;
            _orderService = orderService;
            _couponService = serviceCoupon;
            _shippingService = shippingService;
            _uow = uow;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreatePaymentUrl([FromForm] OrderInfoRequest model) 
        {
            var emailUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;

            OrderInfoModel orderModel = null;

            if(string.IsNullOrEmpty(model.ProductItemId))
            {
                orderModel = await _paymentService.CreatePayment(emailUser, model);
            }
            else
            {
                orderModel = await _paymentService.CreatePaymentOneProductItem(emailUser, model);
            }

            if (model.PaymentMethod == "cod")
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                await _orderService.OrderUpdateStatus(orderModel.OrderId, StaticDefine.Status_Order_Processing, StaticDefine.Status_Payment_Pending);

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

        //Tinh phi ship
        [HttpGet("shipping-fee")]
        public async Task<ActionResult> GetFeeShipping([FromQuery] string[] listProductItem, [FromQuery] int[] quantity)
        {
            if (listProductItem.Length != quantity.Length)
            {
                return BadRequest("Số lượng sản phẩm và danh sách số lượng không khớp.");
            }

            var itemsForShipping = new List<object>();
            int? totalWeight = 0;

            for (int i = 0; i < listProductItem.Length; i++)
            {
                var productItem = await _uow.ProductRepository.GetProductItemByIdAsync(listProductItem[i]);
                if (productItem != null)
                {
                    itemsForShipping.Add(new
                    {
                        name = productItem.NameOption,
                        quantity = quantity[i],
                        height = productItem.HeightSize,
                        weight = productItem.Weight,
                        length = productItem.LengthSize,
                        width = productItem.WidthSize
                    });

                    totalWeight += productItem.Weight * quantity[i];
                }
            }

            var result = await _shippingService.CalculateShippingFeeAsync("a85473ec-2e75-11f0-9b81-222185cb68c8", new
            {
                from_district_id = 1459,
                from_ward_code = "22208",
                service_id = (int?)null,
                service_type_id = 2,
                to_district_id = 1443,
                to_ward_code = "20207",
                cod_failed_amount = 2000,
                coupon = (string?)null,
                weight = totalWeight,
                items = itemsForShipping
            });

            _apiResponse.Result = result.Data;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

    }
}
