using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.Product
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public string? Category_Id { get; set; }
        public string? Brand_Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
