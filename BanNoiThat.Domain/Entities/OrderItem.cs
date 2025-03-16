namespace BanNoiThat.Domain.Entities
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string Order_Id { get; set; }
        public Order Order { get; set; }
        public string NameItem { get; set; }
        public string? ImageItemUrl { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ProductItem_Id { get; set; }
        public ProductItem ProductItem { get; set; }
    }
}
