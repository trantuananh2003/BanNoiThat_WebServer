using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticsService;

        public StatisticsController(IStatisticService statisticsService)
        {
            _statisticsService = statisticsService;
        }


        [HttpGet("revenue")]
        public IActionResult GetRevenue()
        {
            var revenue = _statisticsService.GetRevenue();  // Thống kê doanh thu
            return Ok(revenue);
        }

        [HttpGet("orders")]
        public IActionResult GetNumberOfOrders()
        {
            var products = _statisticsService.GetNumberOfOrders(); 
            return Ok(products);
        }

        [HttpGet("users")]
        public IActionResult Get()
        {
            var products = _statisticsService.GetNumberOfUser();
            return Ok(products);
        }
    }

}
