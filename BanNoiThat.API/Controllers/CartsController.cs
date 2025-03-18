using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

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
        public async Task<ActionResult<ApiResponse>> UpdateCartItem([FromForm] CartItemRequest cartItemRequest)
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            await _cartService.UpdateQuantityItemCartByUserId(userEmail, cartItemRequest);

            return _apiResponse;
        }

        [HttpDelete("{cartId}/cartItems/{cartItemId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RemoveCartItem([FromRoute] string cartId, [FromRoute] string cartItemId)
        {   
            await _cartService.DeleteCartItem(cartId, cartItemId);

            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }
    }
}
