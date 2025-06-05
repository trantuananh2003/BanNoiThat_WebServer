using System.ComponentModel.DataAnnotations;

namespace BanNoiThat.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
