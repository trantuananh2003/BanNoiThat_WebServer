using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Application.DTOs.SaleProgramDtos;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public CategoryResponse? Category { get; set; }
        public BrandResponse? Brand { get; set; }
        public List<ProductItemResponse> ProductItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailUrlSecond { get; set; }
        public string Slug { get; set; } 
    }
}
