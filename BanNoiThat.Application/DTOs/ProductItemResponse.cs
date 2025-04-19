namespace BanNoiThat.Application.DTOs
{
    public class ProductItemResponse
    {
        public string? Id { get; set; }
        public string NameOption { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public string ModelUrl { get; set; }
    }
}
