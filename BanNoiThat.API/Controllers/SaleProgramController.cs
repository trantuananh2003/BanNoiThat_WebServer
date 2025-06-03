using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.SaleProgramDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleProgramController : Controller
    {
        public ApiResponse _apiResponse;
        private IUnitOfWork _uow;
        private IServiceSalePrograms _serviceSalePrograms;

        public SaleProgramController(IUnitOfWork uow, IServiceSalePrograms serviceSalePrograms)
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
            _serviceSalePrograms = serviceSalePrograms;
        }


        [HttpGet()]
        public async Task<ActionResult<ApiResponse>> GetAllSalePrograms()
        {
            var list = await _uow.SaleProgramsRepository.GetAllAsync();
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.Result = list;

            return Ok(_apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetSaleProgram([FromRoute] string Id)
        {
            var entity = await _uow.SaleProgramsRepository.GetAllAsync(x => x.Id == Id);

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.Result = entity;

            return Ok(_apiResponse);
        }

        [HttpPost()]
        public async Task<ActionResult<ApiResponse>> CreateSaleProgram([FromForm] RequestSaleProgram model)
        {
            DateTime startDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime endDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            var saleProgram = new SaleProgram()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Description = model.Description,
                StartDate = startDate,
                EndDate = endDate,
                DiscountType = model.DiscountType,
                DiscountValue = model.DiscountValue,
                MaxDiscount = model.MaxDiscount,
                ApplyType = model.ApplyType,
                ApplyValues = model.ApplyValues,
                IsActive = true
            };

            await _uow.SaleProgramsRepository.CreateAsync(saleProgram);

            await _serviceSalePrograms.ApplySaleProgramsToProduct(saleProgram);

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        [HttpPut("{id}/set-active")]
        public async Task<ActionResult<ApiResponse>> SetIsActiveSaleProgram([FromRoute] string id, [FromForm] Boolean isActive)
        {
            var entity = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == id, tracked: true);
            entity.IsActive = isActive;

            if (isActive)
            {
                await _serviceSalePrograms.GetBackPrice(id);

            }
            else
            {
                await _serviceSalePrograms.PutBackPrice(id);
            }

            return Ok(_apiResponse); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteSalePrograms([FromRoute][Required] string id)
        {
            await _serviceSalePrograms.PutBackPrice(id);

            await _uow.SaleProgramsRepository.DeleteEntityHard(id);
            await _uow.SaveChangeAsync();

            return Ok(_apiResponse);
        }
    }
}
