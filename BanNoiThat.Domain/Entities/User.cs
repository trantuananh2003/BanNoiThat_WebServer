namespace BanNoiThat.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public Boolean IsMale { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; } //admin , customer, staff
    }
}
