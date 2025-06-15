using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OrderService;
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
            string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)!.Value;
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
        public async Task<ActionResult<ApiResponse>> GetListOrderForManager([FromQuery] string orderStatus, [FromQuery] string )
        {
            //string userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)!.Value ;
            var listOrder = await _serviceOrder.GetListOrderForAdmin( orderStatus);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listOrder;
            return Ok(_apiResponse);
        }

        [HttpPatch("cancelOrder/{orderId}")]
        [Authorize(Policy = SDPermissionAccess.CancelOrder)]
        public async Task<ActionResult<ApiResponse>> CancelOrderAsync([FromRoute] string orderId)
        {
            await _serviceOrder.OrderUpdateStatus(orderId, orderStatus: "Cancelled");

            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
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
            var entityOrder = await _uow.OrderRepository.GetOrderIncludeAsync(orderId);
            var listAddress = entityOrder.ShippingAddress.Split('-');

            int totalWeight = entityOrder.OrderItems
                .Select(oi =>
                    (oi.ProductItem.Weight ?? 1) * oi.Quantity
                )
                .Sum();

            var resultId = await ReadFileGHN.GetIDsAsync("./GHN/DuLieuGHN.xlsx", listAddress[1], listAddress[2]);

            var resultGHN = await _serviceShipping.CreateOrderGHN("a85473ec-2e75-11f0-9b81-222185cb68c8", new
            {
                payment_type_id = 2,
                required_note = "KHONGCHOXEMHANG",
                client_order_code = "",
                to_name = entityOrder.UserNameOrder,
                to_phone = entityOrder.PhoneNumber,
                to_address = listAddress[3],
                to_ward_code = resultId.WardCode,
                to_district_id = Convert.ToInt32(resultId.DistrictID),
                insurance_value = 0,
                service_type_id = 2,
                weight = totalWeight,
                items = entityOrder.OrderItems.Select(oi =>
                new
                {
                    name = oi.ProductItem.NameOption,
                    code = oi.ProductItem.Sku,
                    quantity = oi.Quantity,
                    price = (int)oi.Price,
                    length = oi.ProductItem.LengthSize,
                    width = oi.ProductItem.WidthSize,
                    height = oi.ProductItem.HeightSize,
                    weight = oi.ProductItem.Weight ?? 1,
                }).ToList()
            });

            entityOrder.AddressCode = resultGHN.Data.order_code;
            entityOrder.TransferService = "GHN";
            entityOrder.OrderStatus = StaticDefine.Status_Order_Shipping;

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            await _uow.SaveChangeAsync();
            return Ok(_apiResponse);
        }

    }
}
