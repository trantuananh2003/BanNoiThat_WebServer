using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BanNoiThat.Application.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        void AddProductItem(ProductItem productItem);
        void DeleteProductItem(ProductItem productItem);
        Task DeleteSoft(string productId, Boolean isDeleted);
        Task<List<ProductItem>> GetListProductItemByProductIdAsync(string productId);
        Task<PagedList<ProductHomeResponse>> GetPagedListProduct(string stringSearch, int pageSize, int pageCurrent, bool IsDeleted);
        Task<ProductItem> GetProductItemByIdAsync(string id);
        Task UpdatePatchProduct(string id, JsonPatchDocument<Product> productModel);
        void UpdateProductItem(ProductItem productItem);
        Task UpsertProductItemsAsync(string idProduct, IEnumerable<ProductItem> productItems);
    }
}
