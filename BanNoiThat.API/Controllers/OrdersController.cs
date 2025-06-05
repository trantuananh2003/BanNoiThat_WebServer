using Azure;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OrderService;
using BanNoiThat.Application.Service.PaymentService;
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
        private IUnitOfWork _uow;
        private ILogger<OrdersController> _logger;
        private ServiceShipping _serviceShipping;

        public OrdersController(IServiceOrder serviceOrder, IUnitOfWork uow ,ServiceShipping serviceShipping,ILogger<OrdersController> logger)
        {
            _apiResponse = new ApiResponse();
            _serviceOrder = serviceOrder;
            _logger = logger;
            _uow = uow;
            _serviceShipping = serviceShipping;
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

            //User chỉ được quyền hủy
            await _serviceOrder.OrderUpdateStatus(orderId, orderStatus:order.OrderStatus);
            return _apiResponse;
        }

        [HttpPut("{orderId}/approve")]
        public async Task<ActionResult<ApiResponse>> ApproveOrder([FromRoute] string orderId,[FromForm] OrderApproveRequest modelRequest)
        {
            var entity = await _uow.OrderRepository.GetAsync(order => order.Id == orderId, tracked: true);
            entity.AddressCode = modelRequest.AddressCode;
            entity.TransferService = modelRequest.TransferService;
            await _uow.SaveChangeAsync();

            await _serviceOrder.OrderUpdateStatus(orderId, orderStatus: StaticDefine.Status_Order_Shipping);

            return Ok();
        }

        [HttpPut("{orderId}/done")]
        public async Task<ActionResult<ApiResponse>> ApproveOrderSuccess([FromRoute] string orderId)
        {
            var entity = await _uow.OrderRepository.GetAsync(order => order.Id == orderId);
            await _serviceOrder.OrderUpdateStatus(orderId, orderStatus: StaticDefine.Status_Order_Done);

            await _uow.SaveChangeAsync();
            return Ok();
        }

        [HttpPost("{orderId}/create-order-ghn")]
        public async Task<ActionResult<ApiResponse>> CreateOrderGHNAsync([FromRoute] string orderId)
        {
            var entityOrder = await _uow.OrderRepository.GetAsync(x => x.Id == orderId);
            var listAddress = entityOrder.ShippingAddress.Split('-');
            var resultId = await ReadFileGHN.GetIDsAsync("./GHN/DuLieuGHN.xlsx", listAddress[1], listAddress[2]);

            var resultGHN = await _serviceShipping.CreateOrderGHN("a85473ec-2e75-11f0-9b81-222185cb68c8", new
            {
                payment_type_id = 2,
                required_note = "KHONGCHOXEMHANG",
                client_order_code = "",
                to_name = "Tran Tuan Anh",
                to_phone = entityOrder.PhoneNumber,
                to_address = listAddress[3],
                to_ward_code = resultId.WardCode,
                to_district_id = Convert.ToInt32(resultId.DistrictID),
                weight = 200,
                length = 1,
                width = 19,
                height = 10,
                insurance_value = 3000,
                service_type_id = 2,
                items = new[]
                {
                    new
                    {
                        name = "Áo Polo",
                        code = "Polo123",
                        quantity = 1,
                        price = 200000,
                        length = 12,
                        width = 12,
                        height = 12,
                        weight = 1200,
                    }
                }
            });

            entityOrder.AddressCode = resultGHN.Data.order_code;
            entityOrder.TransferService = "GHN";
            entityOrder.OrderStatus = StaticDefine.Status_Order_Shipping;

            await _uow.SaveChangeAsync();
            return Ok();
        }
    }
}
