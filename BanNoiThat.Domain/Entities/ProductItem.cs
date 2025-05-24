namespace BanNoiThat.Domain.Entities
{
    public class ProductItem
    {
        public string Id { get; set; }
        public string? Product_Id { get; set; }
        public Product Product { get; set; }
        public string NameOption { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string Sku { get; set; }
        public string? ImageUrl { get; set; }
        public string? ModelUrl { get; set; }
        public int? LongSize { get; set; }
        public int? WidthSize { get; set; }
        public int? HeightSize { get; set; }
        public string? Colors { get; set; }
    }
}
