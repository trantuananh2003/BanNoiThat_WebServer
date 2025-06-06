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

        [HttpGet("homepage")]
        public async Task<ActionResult<ApiResponse>> GetSaleProgramForHomePage()
        {
            var list = await _uow.SaleProgramsRepository.GetAllAsync();
            var listSaleProgramHomePage = new List<RequestHomePageSaleProgram>();

            foreach (var item in list)
            {
                listSaleProgramHomePage.Add(new RequestHomePageSaleProgram
                {
                    Id = item.Id,
                    Slug = item.Slug,
                    Name = item.Name,
                });
            }

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.Result = listSaleProgramHomePage;

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
                Slug = model.Slug is null ? model.Name.GenerateSlug() : model.Slug,
                IsActive = false,
                Status = StaticDefine.SP_Status_Inactive,
            };

            await _uow.SaleProgramsRepository.CreateAsync(saleProgram);

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

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateSaleProgram([FromRoute] string id, [FromForm] RequestSaleProgram model)
        {
            var entity = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == id, tracked: true);
            var clonedEntity = new SaleProgram
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                DiscountType = entity.DiscountType,
                DiscountValue = entity.DiscountValue,
                MaxDiscount = entity.MaxDiscount,
                ApplyType = entity.ApplyType,
                ApplyValues = entity.ApplyValues,
                IsActive = entity.IsActive
            };

            if (entity == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                });
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.StartDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            entity.EndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            entity.DiscountType = model.DiscountType;
            entity.DiscountValue = model.DiscountValue;
            entity.MaxDiscount = model.MaxDiscount;
            entity.ApplyType = model.ApplyType;
            entity.ApplyValues = string.Join(",", model.ApplyValues.Split(',').Select(v => v.Trim()));
            entity.Status = model.Status;

            await _uow.SaveChangeAsync();

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            });
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
