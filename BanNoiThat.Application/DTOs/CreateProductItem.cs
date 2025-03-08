namespace BanNoiThat.Application.DTOs
{
    public class CreateProductItem
    {
        public int Quantity { get; set; }
        public string? NameOption { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string? Sku { get; set; }
    }
}
