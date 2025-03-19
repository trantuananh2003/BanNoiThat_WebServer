using BanNoiThat.Domain.Interface;

namespace BanNoiThat.Domain.Entities
{
    public class Product : IEntityRecommend
    {
        public string Id { get; set; } = string.Empty;
        public string? Category_Id { get; set; }
        public Category Category { get; set; }
        public string? Brand_Id { get; set; }
        public Brand Brand { get; set; }
        public List<ProductConfig> Configs { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public string? Keyword { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
    }
}
