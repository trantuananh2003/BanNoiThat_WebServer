using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CartsController : Controller
    {
        private ApiResponse _apiResponse;
        private IServiceCarts _cartService;

        public CartsController(IServiceCarts cartService)
        {
            _cartService = cartService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCartAsyncByIdUser(string UserId)
        {
            var modelResponse = await _cartService.GetCartByUserId(UserId);

            _apiResponse.Result = modelResponse;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return _apiResponse;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> UpdateCartItem(string UserId,[FromForm] CartItemRequest cartItemRequest)
        {
            await _cartService.UpdateQuantityItemCartByUserId(UserId, cartItemRequest);

            return _apiResponse;
        }
    }
}
