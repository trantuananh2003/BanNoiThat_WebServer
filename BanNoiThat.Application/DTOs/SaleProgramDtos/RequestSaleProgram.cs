

namespace BanNoiThat.Application.DTOs.SaleProgramDtos
{
    public class RequestSaleProgram
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DiscountType { get; set; } 
        public double DiscountValue { get; set; }
        public double MaxDiscount { get; set; }
        public string ApplyType { get; set; }
        public string ApplyValues { get; set; }
        public string? Slug { get; set; }
        //public Boolean IsActive { get; set; }
    }
}
