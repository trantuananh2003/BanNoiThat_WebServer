﻿using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string? Category_Id { get; set; }
        public string? Brand_Id { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }
}
