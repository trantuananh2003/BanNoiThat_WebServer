using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json.Linq;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticsService;
        private readonly IUnitOfWork _uow;
        private readonly ApiResponse _apiReponse;

        public StatisticsController(IStatisticService statisticsService, IUnitOfWork uow)
        {
            _statisticsService = statisticsService;
            _apiReponse = new ApiResponse();
            _uow = uow;
        }


        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var daynow = DateTime.Today;
            var firstDayLastWeek = DateTime.Today - new TimeSpan((int)daynow.DayOfWeek + 7 - 1, 0, 0, 0);
            var firstDayWeek = DateTime.Today - new TimeSpan((int)daynow.DayOfWeek - 1, 0, 0, 0);

            var listEntity = await _uow.OrderRepository.GetAllAsync(x => x.OrderPaidTime >= firstDayLastWeek && x.OrderStatus == StaticDefine.Status_Order_Done);
            double revenueLastWeek = 0;
            double revenueWeek = 0;

            foreach (var item in listEntity)
            {
                if (item.OrderPaidTime >= firstDayWeek)
                    revenueLastWeek += item.TotalPrice;
                else
                    revenueWeek += item.TotalPrice;
            }

            _apiReponse.Result = new
            {
                revenueLastWeek = revenueLastWeek,
                revenueCurrentWeek = revenueWeek,
            };

            return Ok(_apiReponse);
        }

        [HttpGet("orders")]
        public async Task<ActionResult<ApiResponse>> GetNumberOfOrders()
        {
            var daynow = DateTime.Today;
            var firstDayLastWeek = DateTime.Today - new TimeSpan((int)daynow.DayOfWeek + 7 - 1, 0, 0, 0);
            var firstDayWeek = DateTime.Today - new TimeSpan((int)daynow.DayOfWeek - 1, 0, 0, 0);

            var listEntity = await _uow.OrderRepository.GetAllAsync(x => x.OrderPaidTime >= firstDayLastWeek);

            var entityWeek = listEntity.Where(x => x.OrderPaidTime >= firstDayWeek).Count();
            var entityLastWeek = listEntity.Where(x => x.OrderPaidTime < firstDayWeek).Count();

            _apiReponse.Result = new
            {
                NumberLastWeek = entityLastWeek,
                NumberCurrentWeek = entityWeek,
            };

            _apiReponse.IsSuccess = true;

            return Ok(_apiReponse);
        }

        [HttpGet("chart")]
        public async Task<ActionResult<ApiResponse>> GetRevenueChart([FromQuery] int year)
        {
            var orders = await _uow.OrderRepository.GetAllAsync(x => x.OrderPaidTime.Year == year);
            var arrRevenueOfYear = new double[12];

            foreach(var order in orders)
            {
                arrRevenueOfYear[order.OrderPaidTime.Month] += order.TotalPrice;
            }

            _apiReponse.Result = new
            {
                nameLabel = "Revenue" + year,
                x = arrRevenueOfYear,
            };

            _apiReponse.IsSuccess = true;

            return Ok(_apiReponse);
        }
    }
}
