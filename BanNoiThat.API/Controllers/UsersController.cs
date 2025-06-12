using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BanNoiThat.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public ApiResponse _apiResponse;
        private readonly IServiceUser _serviceUser;
        private IUnitOfWork _unitOfWork;

        public UsersController(IServiceUser serviceUser, IUnitOfWork uof) {
            _apiResponse = new ApiResponse();
            _serviceUser = serviceUser;
            _unitOfWork = uof;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse>> GetInfoUserById([FromRoute] string userId)
        {                
            var modelResponse = await _serviceUser.GetInfoUser(userId);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = modelResponse;

            return _apiResponse;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUser(int pageCurrent, int pageSize, string? stringSearch)
        {
            var modelPagedResponse = await _serviceUser.GetAllUser(stringSearch, pageCurrent, pageSize);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = modelPagedResponse.Items;

            return _apiResponse;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateInfoUser([FromRoute] string id, [FromForm] InfoUserRequest modelRequest)
        {
            var userId = HttpContext.User.Claims.First().Value;

            await _serviceUser.UpdateInfoUser(userId, modelRequest);

            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }

        [HttpPut("{userId}/is-block")]
        [Authorize(Policy = "AllowBlockUser")]
        public async Task<ActionResult<ApiResponse>> BlockUserAsync(string userId,[FromForm] Boolean isBlock)
        {
            await _serviceUser.UpdateUserBlock(userId, isBlock);

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode=HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        [HttpPost("{userId}/set-role")]
        public async Task<ActionResult<ApiResponse>> SetRoleUser(string userId,[FromForm] string? roleId)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);
            _unitOfWork.UserRepository.AttachEntity(userEntity);

            userEntity.Role_Id = roleId;
            await _unitOfWork.SaveChangeAsync();
            return Ok();
        }

        [HttpPost("favorite-product/{productId}")]
        public async Task<ActionResult<ApiResponse>> LikeProduct([FromRoute]string productId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Id)!.Value;

            var entity = new FavoriteProducts() {
                User_Id = userId,
                Product_Id = productId,
            };

            await _unitOfWork.UserRepository.AddFavoriteProduct(entity);
            await _unitOfWork.SaveChangeAsync();
            return _apiResponse;
        }

        //[HttpPatch("{id}/{fieldName}")]
        //public async Task<ActionResult<ApiResponse>> UpdatePatchUser(string id, string fieldName, [FromForm] string value)
        //{
        //    Type userType = typeof(User);

        //    if (userType.GetProperty(fieldName) == null)
        //    {
        //        return BadRequest();
        //    }

        //    _apiResponse.IsSuccess = true;
        //    _apiResponse.StatusCode = HttpStatusCode.OK;
        //    await _serviceUser.UpdateFieldUser(id, fieldName, value);
        //    return _apiResponse;
        //}
    }
}
