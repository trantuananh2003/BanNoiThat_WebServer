using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.DTOs.SaleProgramDtos
{
    public class SaleProgramIncludeProductsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Slug { get; set; }
        public List<ProductHomeResponse> ProductResponses { get; set; }
    }
}
