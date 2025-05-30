using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.CategoryDtos
{
    public class CreateCategoriesRequest
    {
        public string Name { get; set; }
        public string? Parent_Id { get; set; }
        public IFormFile? CategoryImage { get; set; }
        public string? Slug { get; set; }
    }
}
