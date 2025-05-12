using BanNoiThat.API.Model;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ShippingProviderController : Controller
    {
        private ApiResponse _apiResponse;
        private readonly IUnitOfWork _uow;

        public ShippingProviderController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("goship")]
        public async Task<ActionResult> UpdateOrderStautsShipping([FromBody] GoShipOrderStatusResponse model)
        {
            return Ok();
        }
    }
}
