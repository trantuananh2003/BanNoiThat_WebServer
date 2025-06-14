using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CartDtos;
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
        public async Task<ActionResult<ApiResponse>> GetCartAsyncByIdUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)?.Value;

            if(userId is null)
            {
                throw new Exception("Vui lòng đăng nhập để hiện thị giỏ hàng !");
            }

            var modelResponse = await _cartService.GetCartByUserId(userId);

            _apiResponse.Result = modelResponse;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return _apiResponse;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateCartItem([FromForm] CartItemRequest cartItemRequest)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;
            await _cartService.UpdateQuantityItemCartByUserId(userId, cartItemRequest);

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
