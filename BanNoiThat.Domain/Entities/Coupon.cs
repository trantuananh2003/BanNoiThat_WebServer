

namespace BanNoiThat.Domain.Entities
{
    public class Coupon
    {
        public string Id { get; set; }
        public string CouponCode { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } //Filter
        public DateTime EndDate { get; set; } //Filter

        public string Categories { get; set; }

        public string DiscountType { get; set; } //percent, fixed_amount
        public double DiscountValue { get; set; }
        public double MaxDiscount { get; set; }
        public double MinCouponValue { get; set; } 
        public int UsageLimit { get; set; } 
        public int Quantity { get; set; } //Filter

        public List<CouponUsage> CouponUsages { get; set; }
    }
}
