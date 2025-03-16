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
        public async Task<ActionResult<ApiResponse>> GetCartAsyncByIdUser([FromQuery] string email)
       {
            var modelResponse = await _cartService.GetCartByUserEmail(email);

            _apiResponse.Result = modelResponse;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return _apiResponse;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> UpdateCartItem([FromQuery]string email, [FromForm] CartItemRequest cartItemRequest)
        {
            await _cartService.UpdateQuantityItemCartByUserId(email, cartItemRequest);

            return _apiResponse;
        }

        [HttpDelete("{cartId}/cartItems/{cartItemId}")]
        public async Task<ActionResult<ApiResponse>> RemoveCartItem([FromQuery] string email, string cartId, string cartItemId)
        {
            await _cartService.DeleteCartItem(cartId, cartItemId);

            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }
    }
}
