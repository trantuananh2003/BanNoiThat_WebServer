namespace BanNoiThat.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string User_Id { get; set; }
        //Need object User
        public DateTime OrderPaidTime { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public double TotalPrice { get; set; }

        public string PaymentMethod { get; set; } //Cod, Online
        public string? PaymentIntentId { get; set; }
        public string PaymentStatus { get; set; } //Pending, Paid, Failed, Refunded

        public string OrderStatus { get; set; } //Pending (Được bên admin xác nhận sau khi xác nhận chuyển qua trạng thái processing), Processing, Shipping , Done, Cancelled , Returned (Đơn hàng hoàn lại)

        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string? AddressCode { get; set; }
        public string? TransferService {get;set;}
        public string? UserNameOrder { get; set; }
    }
}
