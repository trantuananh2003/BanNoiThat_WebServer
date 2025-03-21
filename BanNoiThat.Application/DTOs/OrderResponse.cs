
namespace BanNoiThat.Application.DTOs
{
    public class OrderResponse
    {
        public string Id { get; set; }
        public string TotalPrice { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderPaidTime { get; set; }
    }

    public class OrderItemResponse
    {
        public string Id { get; set; }
        public string ImageItemUrl { get; set; }
        public string NameItem { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string ProductItem_Id { get; set; }
    }
}
