namespace BanNoiThat.Domain.Entities
{
    public class Brand
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<Product> Products { get; set; }
        public string? Slug { get; set; }
    }
}
