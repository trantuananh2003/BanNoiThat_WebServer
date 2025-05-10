using BanNoiThat.Application.DTOs;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend
{
    public class GetPagedProductsRecommendQuery : IRequest<PagedList<ProductHomeResponse>>
    {
        public int PageSize { get; set; }
        public int PageCurrent { get; set; }
        public string? StringSearch { get; set; }
        public string[] InteractedProductIds { get; set; }
    }
}