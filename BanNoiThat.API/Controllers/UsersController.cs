using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public ApiResponse _apiResponse;
        private readonly IServiceUser _serviceUser;

        public UsersController(IServiceUser serviceUser) {
            _apiResponse = new ApiResponse();
            _serviceUser = serviceUser;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetInfoUserById()
        {
            var userId = HttpContext.User.Claims.First().Value;

            var modelResponse = await _serviceUser.GetInfoUser(userId);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = modelResponse;

            return _apiResponse;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUser()
        {
            var userId = HttpContext.User.Claims.First().Value;

            var modelResponse = await _serviceUser.GetInfoUser(userId);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = modelResponse;

            return _apiResponse;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateInfoUser([FromRoute]string id, [FromForm] InfoUserRequest modelRequest)
        {
            var userId = HttpContext.User.Claims.First().Value;

            await _serviceUser.UpdateInfoUser(userId, modelRequest);

            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }


    }
}
