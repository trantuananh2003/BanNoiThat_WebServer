﻿
namespace BanNoiThat.Application.DTOs.AuthDtos
{
    public class ResetPasswordDto
    {

        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
