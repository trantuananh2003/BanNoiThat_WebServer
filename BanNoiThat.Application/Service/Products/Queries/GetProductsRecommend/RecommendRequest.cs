
namespace BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend
{
    public class RecommendRequest
    {
        public Boolean IsSpecial { get; set; } = false;
        public List<string>? InteractedProductIds { get; set; }
    }
}
