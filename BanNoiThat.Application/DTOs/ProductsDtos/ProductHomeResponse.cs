using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class ProductHomeResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailUrlSecond { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string? Keyword { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalSoldQuantity { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public SaleProgram SaleProgram { get; set; }
        public Boolean IsHaveModel3D { get; set; }
    }
}
