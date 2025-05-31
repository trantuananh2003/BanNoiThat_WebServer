using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class CreateProductItem
    {
        public int Quantity { get; set; }
        public string? NameOption { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string? Sku { get; set; }
        public IFormFile? Image { get; set; }
    }
}
