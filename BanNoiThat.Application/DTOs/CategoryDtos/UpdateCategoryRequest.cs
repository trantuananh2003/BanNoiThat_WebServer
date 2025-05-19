using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanNoiThat.Application.DTOs.CategoryDtos
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; }
        public string? Slug { get; set; }
        public IFormFile? FileImage { get; set; }
        public bool? IsShow { get; set; }
    }
}
