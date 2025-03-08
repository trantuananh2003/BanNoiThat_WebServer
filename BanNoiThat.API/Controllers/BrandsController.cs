using AutoMapper;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Interfaces.IService;
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

        public BrandsController(IServiceBrands serviceBrands, ILogger<CategoriesController> logger,
            IMapper mapper)
        {
            _serviceBrands = serviceBrands;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllCategory()
        {
            var modelsResponse = await _serviceBrands.GetAllBrandAsync();

            if (modelsResponse.IsNullOrEmpty())
            {
                _logger.LogWarning("END: Get list models reponse null");
            }

            _apiResponse.Result = modelsResponse;
            _apiResponse.IsSuccess = true;

            _logger.LogInformation("END: Get list models reponse");

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

    }
}
