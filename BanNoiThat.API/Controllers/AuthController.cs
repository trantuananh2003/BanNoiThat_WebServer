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
using Microsoft.AspNetCore.Identity;

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
        private IPasswordHasher<User> _passwordHasher;

        public AuthController(IUnitOfWork uow, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
            _configuration = configuration;
            _secretKey = _configuration.GetValue<string>("ApiSetting:Secret");
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            //Check account
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == loginRequest.Email);
            if (userEntity == null)
            {
                return Unauthorized();
            }

            var resultValidate = _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, loginRequest.Password);
            if(resultValidate == PasswordVerificationResult.Failed)
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
                    new Claim(StaticDefine.Claim_User_Role, !string.IsNullOrEmpty(userEntity.Role_Id) ? userEntity.Role_Id : "")
                    //new Claim(SDClaimAccess.ManageUser, SDClaimAccess.ClaimBlockUser)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var loginResponse = new LoginResponse()
            {
                Email = userEntity.Email,
                FullName = userEntity.FullName,
                UserId = userEntity.Id,
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
                Role_Id = StaticDefine.Role_Customer,
                IsBlocked = false,
                IsMale = false
            };

            var passwordHash = _passwordHasher.HashPassword(userEntity, registerRequest.Password);
            userEntity.PasswordHash = passwordHash;

            await _uow.UserRepository.CreateAsync(userEntity);
            await _uow.SaveChangeAsync();
            return Ok();
        }
    }
}
