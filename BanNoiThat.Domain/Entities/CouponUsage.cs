namespace BanNoiThat.Domain.Entities
{
    public class CouponUsage
    {
        public string Id { get; set; }
        public string Coupon_Id { get; set; }
        public Coupon Coupon { get; set; }
        public string CouponCode { get; set; }
        public string User_Id { get; set; }
        public User User { get; set; }
        public string Order_Id { get; set; }
        public DateTime UsageDate { get; set; }
        public double DiscountAmount { get; set; }
    }
}
