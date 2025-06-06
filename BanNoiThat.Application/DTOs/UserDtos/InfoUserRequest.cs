using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.User
{
    public class InfoUserRequest
    {
        public string? FullName { get; set; }
        public Boolean IsMale { get; set; }
        public DateTime BirthDay { get; set; }
        public Boolean IsOnlyUpdateInfo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
    }
}
