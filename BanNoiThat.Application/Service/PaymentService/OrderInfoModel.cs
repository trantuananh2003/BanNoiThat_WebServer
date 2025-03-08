namespace BanNoiThat.Application.Service.PaymentService
{
    public class OrderInfoModel
    {
        public string? OrderId { get; set; }
        public double? TotalPrice { get; set; }
        public string FullName { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderInformation { get; set; }
    }
}
