namespace BanNoiThat.Domain.Entities
{
    public class Brand
    {
        public string Id { get; set; }
        public string? BrandName { get; set; }
        public List<Product> Products { get; set; }
    }
}
