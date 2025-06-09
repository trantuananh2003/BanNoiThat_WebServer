using AutoMapper;
using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.ReviewDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.BrandService;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : Controller
    {
        private readonly IMapper _mapper;
        private ApiResponse _apiResponse;
        private IUnitOfWork _uow;

        public ReviewsController(IUnitOfWork uow,
            IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ApiResponse>> GetAllReviewOfProduct([FromRoute] string id)
        { 
            var modelsResponse = await _uow.ProductRepository.GetAsync(x => x.Id == id);

            _apiResponse.Result = modelsResponse;

            return Ok(_apiResponse);
        }

        [HttpPost("product/{productId}")]
        public async Task<ActionResult<ApiResponse>> ReviewProductAsync([FromRoute] string productId, [FromForm] RateCreateDto model )
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)?.Value;

            if (userId == null)
            {
                _apiResponse.IsSuccess = false;
                return Unauthorized(_apiResponse);
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                User_Id = userId,
                Product_Id = productId,
                Comment = model.Comment,
                Rate = model.Rate,
            };


            _apiResponse.IsSuccess = true;
            _apiResponse.Result = review;
            return Ok(_apiResponse);
        }
    }
}
