using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetListOrder([FromQuery] string orderStatus)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)!.Value;

            var listOrder = await _serviceOrder.GetListOrder(userId, orderStatus);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listOrder;
            return Ok(_apiResponse);
        }

        [HttpPatch()]
        public async Task<ActionResult<ApiResponse>> UpdateInfoOrder()
        {
            return _apiResponse;
        }
    }
}
