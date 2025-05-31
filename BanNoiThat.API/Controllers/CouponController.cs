using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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

        public CouponController(IUnitOfWork uow)
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
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
                Code = GenerateRandomString.Generate(10),
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
    }
}
