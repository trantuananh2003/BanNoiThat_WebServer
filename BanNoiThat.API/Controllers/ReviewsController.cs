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
        public async Task<ActionResult<ApiResponse>> GetAllReviewOfProduct([FromRoute] string productId)
        { 
            var model = await _uow.ReviewRepository.GetAllAsync(x => x.Product_Id == productId);

            _apiResponse.Result = model;

            return Ok(_apiResponse);
        }

        [HttpPost()]
        public async Task<ActionResult<ApiResponse>> ReviewProductAsync([FromForm] RateCreateDto model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)?.Value;

            var productItem = await _uow.ProductItemRepository.GetAsync(x => x.Id == model.ProductItemId, includeProperties:"Product");
            var orderItem = await _uow.OrderRepository.GetOrderItemById(model.OrderItemId);
            var user = await _uow.UserRepository.GetAsync(x => x.Id == userId);

            if (userId == null)
            {
                _apiResponse.IsSuccess = false;
                return Unauthorized(_apiResponse);
            }

            orderItem.IsComment = true;

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                User_Id = userId,
                NameUser = user.FullName,
                Comment = model.Comment,
                Rate = model.Rate,
                Product_Id = productItem.Product.Id,
                ProductItem_Id = model.ProductItemId,
                CreateAt = DateTime.UtcNow,
                OrderItem_Id = orderItem.Id,
                IsShow = true,
            };

            await _uow.ReviewRepository.CreateAsync(review);
            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = review;
            return Ok(_apiResponse);
        }
    }
}
