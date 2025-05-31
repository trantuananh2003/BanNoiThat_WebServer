namespace BanNoiThat.Application.DTOs.CouponDtos
{
    public class CreateCouponRequestDto
    {
        public string? Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string DiscountType { get; set; } //percent, fixed_amount
        public double DiscountValue { get; set; }
        public double MaxDiscount { get; set; }
        public double MinCouponValue { get; set; }
        public int UsageLimit { get; set; }
        public int Quantity { get; set; }
    }
}
