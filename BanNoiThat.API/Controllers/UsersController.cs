using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Application.Interfaces.IService;
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
        public async Task<ActionResult<ApiResponse>> GetAllUser(int pageCurrent, int pageSize, string? stringSearch)
        {
            //var userId = HttpContext.User.Claims.First().Value;

            var modelPagedResponse = await _serviceUser.GetAllUser(stringSearch, pageCurrent, pageSize);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = modelPagedResponse.Items;

            return _apiResponse;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateInfoUser([FromRoute] string id, [FromForm] InfoUserRequest modelRequest)
        {
            var userId = HttpContext.User.Claims.First().Value;

            await _serviceUser.UpdateInfoUser(userId, modelRequest);

            _apiResponse.IsSuccess = true;
            return _apiResponse;
        }

        [HttpPatch("{id}/{fieldName}")]
        public async Task<ActionResult<ApiResponse>> UpdatePatchUser(string id, string fieldName, [FromForm] string value)
        {
            Type userType = typeof(User);

            if (userType.GetProperty(fieldName) == null)
            {
                return BadRequest();
            }

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            await _serviceUser.UpdateFieldUser(id, fieldName, value);
            return _apiResponse;
        }
    }
}
