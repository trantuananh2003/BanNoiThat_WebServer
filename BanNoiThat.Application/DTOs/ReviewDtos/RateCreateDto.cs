namespace BanNoiThat.Application.DTOs.ReviewDtos
{
    public class RateCreateDto
    {
        public string OrderItemId { get; set; }
        public string ProductItemId { get; set; }
        public string Comment { get; set; }
        public double Rate { get; set; }
    }
}
