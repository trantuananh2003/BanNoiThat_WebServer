namespace BanNoiThat.Domain.Entities
{
    public class SaleProgram
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DiscountType { get; set; } //percent, fixed_amount
        public double DiscountValue { get; set; }
        public double MaxDiscount { get; set; } 
        public string ApplyType { get; set; }
        public string ApplyValues { get; set; }
        public Boolean IsActive { get; set; }

        public List<ProductItem> ProductItems { get; set; }
    }
}
