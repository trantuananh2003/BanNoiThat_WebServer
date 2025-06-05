namespace BanNoiThat.Application.Service.PaymentMethod.MomoService.Momo
{
    public class OrderInfoRequest
    {
        public string FullName { get; set; }
        public string PaymentMethod { get; set; } //COD, ONLINE
        public string ShippingAddress { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string PhoneNumber { get; set; }
        public List<string>? CouponCodes { get; set; }
    }
}
