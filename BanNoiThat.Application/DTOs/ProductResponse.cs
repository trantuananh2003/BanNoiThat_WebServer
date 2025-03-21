using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.Service.CategoriesService;

namespace BanNoiThat.Application.DTOs
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
        public string Slug { get; set; }
    }
}
