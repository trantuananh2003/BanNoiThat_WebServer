using BanNoiThat.Application.DTOs;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BanNoiThat.Application.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<ProductHomeResponse>> GetPagedListProduct(string stringSearch, int pageSize, int pageCurrent);
        Task<ProductItem> GetProductItemByIdAsync(string id);
        Task UpdatePatchProduct(string id, JsonPatchDocument<Product> productModel);
        Task UpsertProductItemsAsync(string idProduct, IEnumerable<ProductItem> productItems);
    }
}
