using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private ApiResponse _apiResponse;
        private IServiceOrder _serviceOrder;
        private ILogger<OrdersController> _logger;

        public OrdersController(IServiceOrder serviceOrder, ILogger<OrdersController> logger)
        {
            _apiResponse = new ApiResponse();
            _serviceOrder = serviceOrder;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetInfoOrderById(string id)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "userId")!.Value;

            var orderEntity = await _serviceOrder.GetDetailOrderById(id, userId);

            _apiResponse.Result = orderEntity;
            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }

        [HttpGet("customer")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetListOrderForUser([FromQuery] string orderStatus)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)!.Value;

            var listOrder = await _serviceOrder.GetListOrderForClient(userId, orderStatus);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listOrder;
            return Ok(_apiResponse);
        }

        [HttpGet("manager")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetListOrderForManager([FromQuery] string orderStatus)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)!.Value;

            var listOrder = await _serviceOrder.GetListOrderForAdmin(userId, orderStatus);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listOrder;
            return Ok(_apiResponse);
        }

        [HttpPatch("{orderId}/orderStatus")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateOrderStatus([FromRoute] string orderId, [FromForm] OrderPatchRequest order)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;
            var roleUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;

            //Chỉ có user mới đc hủy đơn
            await _serviceOrder.OrderUpdateStatus(orderId, order.OrderStatus);
            return _apiResponse;
        }
    }
}
