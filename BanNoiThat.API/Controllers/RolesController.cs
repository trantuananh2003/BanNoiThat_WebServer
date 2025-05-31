using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.RoleDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RolesController : Controller
    {
        public ApiResponse _apiResponse;
        private IUnitOfWork _uow;

        public RolesController(IUnitOfWork uof)
        {
            _apiResponse = new ApiResponse();
            _uow = uof;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetRolesAsync()
        {
            var listEntity = await _uow.RolesRepository.GetAllAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = listEntity;

            return Ok(_apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetRoleAsync([FromRoute] string id)
        {
            var entity = await _uow.RolesRepository.GetAsync(x => x.Id == id, includeProperties: "RoleClaims");

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = entity;

            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddRoleAsync([FromForm] string NameRole)
        {
            await _uow.RolesRepository.CreateAsync(new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = NameRole,
                NameNormalized = NameRole
            });

            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        [HttpPost("{RoleId}/role-claims")]
        public async Task<ActionResult<ApiResponse>> AddClaimToRoleAsync([FromRoute] string RoleId,[FromForm] UpsertRolePermission modelRequest)
        {
            _uow.RolesRepository.AddRoleClaim(RoleId,SDPermissionAccess.Manage, modelRequest.PermissionName);

            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        [HttpDelete("{RoleId}/role-claims/{RoleClaimId}")]
        public async Task<ActionResult<ApiResponse>> AddClaimToRoleAsync([FromRoute] string RoleClaimId)
        {
            await _uow.RolesRepository.DeleteRoleClaim(RoleClaimId);

            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpGet("permissions")]
        public async Task<ActionResult<ApiResponse>> GetAllPermission()
        {
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = SDPermissionAccess.Permissions;

            return Ok(_apiResponse);
        }
    }
}
