

using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceProduct
    {
        Task<PagedList<ProductHomeResponse>> HandleRecommend(GetPagedProductsRecommendQuery request);
    }
}
