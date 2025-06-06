

namespace BanNoiThat.Application.DTOs.User
{
    public class InfoUserResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Boolean IsMale { get; set; }
        public string Birthday { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
