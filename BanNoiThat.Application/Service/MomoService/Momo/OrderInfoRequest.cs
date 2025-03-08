namespace BanNoiThat.Application.Service.MomoService.Momo
{
    public class OrderInfoRequest
    {
        public string FullName { get; set; }
        public string PaymentMethod { get; set; } //COD, ONLINE
        public string ShippingAddress { get; set; } 
        public string PhoneNumber { get; set; }
    }
}
