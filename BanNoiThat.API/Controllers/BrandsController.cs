using AutoMapper;
using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.BrandService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : Controller
    {
        private readonly IServiceBrands _serviceBrands;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;
        private ApiResponse _apiResponse;
        private IUnitOfWork _uow;

        public BrandsController(IServiceBrands serviceBrands, ILogger<CategoriesController> logger,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _serviceBrands = serviceBrands;
            _mapper = mapper;
            _logger = logger;
            _uow = uow;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllBrand()
        {
            var modelsResponse = await _serviceBrands.GetAllBrandAsync();

            if (modelsResponse.Any())
            {
                _logger.LogWarning("END: Get list models reponse null");
            }

            _apiResponse.Result = modelsResponse;
            _apiResponse.IsSuccess = true;

            _logger.LogInformation("END: Get list models reponse");

            return Ok(_apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetBrandById([FromRoute] string id)
        {
            var modelReponse = await _uow.BrandRepository.GetAsync(x => x.Id == id);

            _apiResponse.Result = new
            {
                Id = modelReponse.Id,
                Name = modelReponse.Name,
                Slug = modelReponse.Slug,
            };

            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateBrand([FromForm] CreateBrandRequest modelRequest)
        {
            await _serviceBrands.CreateBrandAsync(modelRequest);

            _logger.LogInformation("END: Create Brand Success");

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateBrand(string id, [FromForm] UpdateBrandRequest modelRequest )
        {
            await _serviceBrands.UpdateBrandAsync(id, modelRequest);

            _logger.LogInformation("END: Update Brand Success");

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteBrandHard(string id)
        {
            await _serviceBrands.DeleteBrandAsync(id);

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}
