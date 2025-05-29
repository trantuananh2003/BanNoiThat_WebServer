using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class ProductItemRequest
    {
        public string? Id { get; set; }
        public string NameOption { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string Sku { get; set; }
        public IFormFile? ImageProductItem { get; set; }
        public bool IsDelete { get; set; }
        public int? HeightSize { get; set; }
        public int? WidthSize { get; set; }
        public int? LengthSize { get; set; }
    }
}
