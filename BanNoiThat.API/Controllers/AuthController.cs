﻿using BanNoiThat.API.Model;
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
using System.Web;
using BanNoiThat.Application.Service.MailsService;
using BanNoiThat.Application.DTOs.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;

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
        private readonly SendMailService _sendMailService;
        private readonly DataProtectorTokenProviderService _dataProtectorTokenProviderService;

        public AuthController(IUnitOfWork uow, IConfiguration configuration, IPasswordHasher<User> passwordHasher, 
            SendMailService sendMailService,  DataProtectorTokenProviderService dataProtectorTokenProviderService )
        {
            _apiResponse = new ApiResponse();
            _uow = uow;
            _configuration = configuration;
            _secretKey = _configuration.GetValue<string>("ApiSetting:Secret");
            _passwordHasher = passwordHasher;
            _sendMailService = sendMailService;
            _dataProtectorTokenProviderService = dataProtectorTokenProviderService;
        }

        [HttpPost("login")] //76
        public async Task<ActionResult<ApiResponse>> Login([FromForm] LoginRequestDto loginRequest)
        {

            var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == loginRequest.Email);
            var resultValidate = _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, loginRequest.Password);
            if (resultValidate == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Thông tin không chính xác");
            }

            if (userEntity == null) return Unauthorized("Người dùng không tồn tại.");
            if (!userEntity.EmailConfirmed) return Unauthorized("Vui lòng xác thực email của bạn.");
            if (userEntity.IsBlocked) return Unauthorized("Người dùng đã bị khóa.");


            // Generate JWT using the shared method
            string token = GenerateJwt(userEntity.Id, userEntity.Email, userEntity.FullName, userEntity.Role_Id);

            var loginResponse = new LoginResponse
            {
                Email = userEntity.Email,
                FullName = userEntity.FullName,
                UserId = userEntity.Id,
                Token = token
            };

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        [HttpPost("login-google")]
        public async Task<ActionResult> LoginGoogle([FromForm] GoogleLoginRequest request)
        {
            try
            {
                var googleClientId = _configuration.GetValue<string>("Google:ClientId");
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId }
                });

                var email = payload.Email;
                var name = payload.Name;

                // Kiểm tra người dùng trong cơ sở dữ liệu
                var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == email);

                if (userEntity == null)
                {
                    // Tạo người dùng mới
                    userEntity = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = name,
                        Email = email,
                        Role_Id = "customer",
                        IsBlocked = false,
                        IsMale = false
                    };

                    var passwordHash = _passwordHasher.HashPassword(userEntity, Guid.NewGuid().ToString());
                    userEntity.PasswordHash = passwordHash;

                    await _uow.UserRepository.CreateAsync(userEntity);
                    await _uow.SaveChangeAsync();
                }

                // Generate JWT using the shared method
                string token = GenerateJwt(userEntity.Id, userEntity.Email, userEntity.FullName, userEntity.Role_Id);

                var loginResponse = new LoginResponse
                {
                    Email = userEntity.Email,
                    FullName = userEntity.FullName,
                    UserId = userEntity.Id,
                    Token = token
                };

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = loginResponse;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages.Add("Đăng nhập google thất bại");
                return BadRequest(_apiResponse);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] RegisterRequestDto registerRequest)
        {
            var entity = (await _uow.UserRepository.GetAsync(x => x.Email == registerRequest.Email) != null
                    ? throw new Exception("Email đã được đăng ký.") : Task.CompletedTask);

            var userEntity = new User()
            {
                Id = Guid.NewGuid().ToString(), FullName = registerRequest.FullName,
                Email = registerRequest.Email, Role_Id = "customer",
                IsBlocked = false,
                IsMale = false,
            };

            var passwordHash = _passwordHasher.HashPassword(userEntity, registerRequest.Password);
            userEntity.PasswordHash = passwordHash;

            await _uow.UserRepository.CreateAsync(userEntity);

            var token = _dataProtectorTokenProviderService.GenerateToken(userEntity.Id, "confirmpassword");
            string encodedToken = HttpUtility.UrlEncode(token);

            string callBackUrl = StaticDefine.SD_URL_LINK_CONFIRMPASSWORD + $"?token={encodedToken}&email={userEntity.Email}";

            var mailContent = new MailContent
            {
                To = userEntity.Email,
                Subject = "Xác nhận mật khẩu",
                Body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; border-radius: 8px;'>
                    <h2 style='color: #333;'>Xác nhận mật khẩu đăng ký</h2>
                    <p>Xin chào {userEntity.FullName},</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản với chúng tôi. Vui lòng nhấn vào nút bên dưới để xác nhận mật khẩu của bạn:</p>
                    <a href='{callBackUrl}' 
                       style='display: inline-block; padding: 10px 20px; margin: 20px 0; color: #fff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>
                        Xác nhận mật khẩu
                    </a>
                    <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                    <p>Trân trọng,<br/>Đội ngũ hỗ trợ</p>
                </div>"
            };

            await _sendMailService.SendMail(mailContent);

            await _uow.SaveChangeAsync();
            return Ok();
        }

        [HttpGet("confirm-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromQuery] string token, [FromQuery] string email)
        {
            
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == email, tracked: true);
            if (userEntity is null)
            {
                return BadRequest("Invalid Request");
            }

            string userId;
            var isValid = _dataProtectorTokenProviderService.ValidateToken(token, "confirmpassword", out userId);

            if (isValid && userEntity.Id == userId)
            {
                userEntity.EmailConfirmed = true;
                await _uow.SaveChangeAsync();

                _apiResponse.IsSuccess = true;
                Response.Redirect("http://localhost:3005/");
                return null;
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Xác thực thất bại");
                return BadRequest(_apiResponse);
            }
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var userFromDb = await _uow.UserRepository.GetAsync(x => x.Email == email);

            if (userFromDb == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Không tìm thấy tài khoản");
                return BadRequest(_apiResponse);
            }

            var token = _dataProtectorTokenProviderService.GenerateToken(userFromDb.Id, "ResetPassword");
            string encodedToken = HttpUtility.UrlEncode(token);


            string callBackUrl = StaticDefine.SD_URL_LINK_RESETPASSWORD + $"?token={encodedToken}&email={userFromDb.Email}";

            await _sendMailService.SendMail(new MailContent { To = email, Body = $"<a href='{callBackUrl}'>clicking here</a>.", Subject = "Đổi mật khẩu" });

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromForm] ResetPasswordDto resetPassordDto)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Email == resetPassordDto.Email, tracked: true);
            if (userEntity is null)
            {
                return BadRequest("Invalid Request");
            }

            string userId;
            var isValid = _dataProtectorTokenProviderService.ValidateToken(resetPassordDto.Token, "ResetPassword", out userId);

            if (isValid && userEntity.Id == userId)
            {
                var passwordHash = _passwordHasher.HashPassword(userEntity, resetPassordDto.NewPassword);
                userEntity.PasswordHash = passwordHash;
                await _uow.SaveChangeAsync();

                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Xác thực thất bại");
                return BadRequest(_apiResponse);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromForm] ChangePasswordRequest model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == StaticDefine.Claim_User_Id)?.Value;

            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId, tracked: true);

            var passwordHash = _passwordHasher.HashPassword(userEntity, model.NewPassword);
            userEntity.PasswordHash = passwordHash;

            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        private string GenerateJwt(string userId, string email, string fullName, string roleId)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(StaticDefine.Claim_User_Id, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(StaticDefine.Claim_FullName, fullName),
                    new Claim(StaticDefine.Claim_User_Role, roleId ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}