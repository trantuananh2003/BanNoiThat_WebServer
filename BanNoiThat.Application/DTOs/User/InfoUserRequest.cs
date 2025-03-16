using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.User
{
    public class InfoUserRequest
    {
        public string FullName { get; set; }
        public Boolean IsMale { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
