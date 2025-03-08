using AutoMapper;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Service.CategoriesService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IServiceCategories _serviceCategories;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;
        private ApiResponse _apiResponse;

        public CategoriesController(IServiceCategories serviceCategories, ILogger<CategoriesController> logger,
            IMapper mapper)
        {
            _serviceCategories = serviceCategories;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllBrand()
        {
            var modelsResponse = await _serviceCategories.GetAllCategoriesAsync();

            if (modelsResponse.IsNullOrEmpty()) {
                _logger.LogWarning("END: Get list models reponse null");
            }

            _apiResponse.Result = modelsResponse;
            _apiResponse.IsSuccess = true;

            _logger.LogInformation("END: Get list models reponse");

            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCategory([FromForm] CreateCategoriesRequest model)
        {
            await _serviceCategories.CreateCategoryAsync(model);

            _logger.LogInformation("END: Create Category Success");

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }
    }
}
