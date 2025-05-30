using AutoMapper;
using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Infrastructure.SqlServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IServiceCategories _serviceCategories;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;
        private ApiResponse _apiResponse;
        private IUnitOfWork _uow;

        public CategoriesController(IServiceCategories serviceCategories, ILogger<CategoriesController> logger, IUnitOfWork uow,
            IMapper mapper)
        {
            _serviceCategories = serviceCategories;
            _mapper = mapper;
            _logger = logger;
            _uow = uow;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("client")]
        public async Task<ActionResult<ApiResponse>> GetAllCategoryForClient()
        {
            var modelsResponse = await _serviceCategories.GetCategoriesForClientAsync();

            if (modelsResponse.Any()) {
                _logger.LogWarning("END: Get list models reponse null");
            }

            _apiResponse.Result = modelsResponse;
            _apiResponse.IsSuccess = true;

            _logger.LogInformation("END: Get list models reponse");

            return Ok(_apiResponse);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<ApiResponse>> GetAllCategoryForAdmin()
        {
            var modelsResponse = await _serviceCategories.GetCategoriesForAdminAsync();
            
            //Có thể bị lỗi
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
        public async Task<ActionResult<ApiResponse>> GetCategoryById([FromRoute]string id)
        {
            var modelReponse = await _uow.CategoriesRepository.GetAsync(x => x.Id == id);

            _apiResponse.Result = new {
                Id = modelReponse.Id,
                Name = modelReponse.Name,
                Slug = modelReponse.Slug,
                CategoryUrlImage = modelReponse.CategoryUrlImage,
            };
            return Ok(_apiResponse);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory(string id, [FromForm] UpdateCategoryRequest modelRequest)
        {
            await _serviceCategories.UpdateCategoryAsync(id, modelRequest);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategoryHardAsync(string id)
        {
            await _serviceCategories.DeleteCategoryHardAsync(id);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpGet("homepage")]
        public async Task<ActionResult<ApiResponse>> GetQuantityCategories(int number)
        {
            var listCategories = await _uow.CategoriesRepository.GetAllAsync(x => x.IsShow != 0);
            _apiResponse.Result = listCategories;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }


    }
}
