namespace BanNoiThat.Application.DTOs
{
    public class ProductHomeResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string? Keyword { get; set; }
    }
}
