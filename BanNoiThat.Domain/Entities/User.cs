namespace BanNoiThat.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public Boolean IsMale { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Role_Id { get; set; }
        public Role Role { get; set; }
        public Boolean? IsBlocked { get; set; }
        public string? Address { get; set; }
        public ICollection<FavoriteProducts> LikesProduct { get; set; }
        public string? PhoneNumber { get; set; }

        public List<CouponUsage> CouponUsages { get; set; }
    }
}
