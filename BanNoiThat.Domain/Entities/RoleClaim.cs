namespace BanNoiThat.Domain.Entities
{
    public class RoleClaim
    {
        public string Id { get; set; }
        public string Role_Id { get; set; }
        public Role Role { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
