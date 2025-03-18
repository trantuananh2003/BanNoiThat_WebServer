using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs.Auth;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BanNoiThat.Application.Common;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private ApiResponse _apiResponse;
        private IConfiguration _configuration;
        private IUnitOfWork _uow;
        private string _secretKey;

        public AuthController(IUnitOfWork uow, IConfiguration configuration)
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
            _configuration = configuration;
            _secretKey = _configuration.GetValue<string>("ApiSetting:Secret");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            //Check account
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password);
            if (userEntity == null)
            {
                return Unauthorized();
            }

            //Generate JWT
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(StaticDefine.Claim_User_Id, userEntity.Id),
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim(StaticDefine.Claim_FullName, userEntity.FullName),
                    new Claim(StaticDefine.Claim_User_Role, userEntity.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var loginResponse = new LoginResponse()
            {
                Email = userEntity.Email,
                FullName = userEntity.FullName,
                Token = tokenHandler.WriteToken(token)
            };

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] RegisterRequest registerRequest)
        {
            var userEntity = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Role = StaticDefine.Role_Customer,
            };

            await _uow.UserRepository.CreateAsync(userEntity);
            await _uow.SaveChangeAsync();
            return Ok();
        }
    }
}
