namespace BanNoiThat.Domain.Entities
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameNormalized { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
    }
}
