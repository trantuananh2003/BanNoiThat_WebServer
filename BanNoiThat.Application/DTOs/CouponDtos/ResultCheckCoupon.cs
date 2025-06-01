namespace BanNoiThat.Application.DTOs.CouponDtos
{
    public class ResultCheckCoupon
    {
        public bool IsCanApply { get; set; }
        public double AmountDiscount { get; set; }
        public string NameCoupon { get; set; }
        public string Coupon_Id { get; set; }
        public string CouponCode { get; set; }
    }
}
