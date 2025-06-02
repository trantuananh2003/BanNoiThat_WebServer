using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : Controller
    {
        private ApiResponse _apiResponse;
        private IUnitOfWork _uow;
        private IServiceCoupon _serviceCoupon;
        private IServiceCarts _serviceCart;

        public CouponController(IUnitOfWork uow, IServiceCoupon serviceCoupon, IServiceCarts serviceCart)
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
            _serviceCoupon = serviceCoupon;
            _serviceCart = serviceCart;
        }

        [HttpGet("checkcoupon")]
        public async Task<ActionResult<ApiResponse>> CheckCouponAsync([FromQuery] string codeCoupon, [FromQuery] List<string> couponsAdded)
        {

            await _serviceCoupon.CheckApplyTogether(couponsAdded, codeCoupon);
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)?.Value;
            var cart = await _uow.CartRepository.GetCartByIdUser(userId);
            var result = await _serviceCoupon.CheckCouponInOrder(codeCoupon, cart);

            _apiResponse.Result = new
            {
                codeCoupon = codeCoupon,
                isCanApply = result.IsCanApply,
                amountDiscount = result.AmountDiscount,
                nameCoupon = result.NameCoupon,
            };

            return Ok(_apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCouponsAsync()
        {
            var listEntity = await _uow.CouponsRepository.GetAllAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listEntity;

            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCouponAsync([FromForm] CreateCouponRequestDto model)
        {
            DateTime startDate = DateTime.ParseExact(model.StartDate,
               "dd-MM-yyyy HH:mm:ss",
               CultureInfo.InvariantCulture);

            DateTime endDate = DateTime.ParseExact(model.EndDate,
                                       "dd-MM-yyyy HH:mm:ss",
                                       CultureInfo.InvariantCulture);

            await _uow.CouponsRepository.CreateAsync(new Coupon {
                Id = Guid.NewGuid().ToString(),
                CouponCode = GenerateRandomString.Generate(10),
                Description = model.Description,
                StartDate = startDate,
                EndDate = endDate,
                DiscountType = model.DiscountType,
                DiscountValue = model.DiscountValue,
                MaxDiscount = model.MaxDiscount,
                MinCouponValue = model.MinCouponValue,
                UsageLimit = model.UsageLimit,
                Quantity = model.Quantity,
            });

            await _uow.SaveChangeAsync();

            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCouponAsync([FromRoute]string id)
        {
            var enity = await _uow.CouponsRepository.GetAsync(x => x.Id == id);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = enity;

            return Ok(_apiResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCouponAsync([FromRoute] string id, [FromForm] CreateCouponRequestDto modelUpdate)
        {
            var entity = await _uow.CouponsRepository.GetAsync(x => x.Id == id, tracked: true);

            if (entity == null)
            {
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            // Cập nhật thông tin từ DTO vào entity
            entity.Description = modelUpdate.Description;
            // Định dạng ngày bạn mong đợi từ client
            var format = "dd-MM-yyyy HH:mm:ss";
            var culture = CultureInfo.InvariantCulture;

            entity.StartDate = DateTime.ParseExact(modelUpdate.StartDate, format, culture);
            entity.EndDate = DateTime.ParseExact(modelUpdate.EndDate, format, culture);

            entity.DiscountType = modelUpdate.DiscountType;
            entity.DiscountValue = modelUpdate.DiscountValue;
            entity.MaxDiscount = modelUpdate.MaxDiscount;
            entity.MinCouponValue = modelUpdate.MinCouponValue;
            entity.UsageLimit = modelUpdate.UsageLimit;
            entity.Quantity = modelUpdate.Quantity;
            entity.TypeCoupon = modelUpdate.TypeCoupon;

            // Cập nhật vào DB
            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = entity;

            return Ok(_apiResponse);
        }
    }
}