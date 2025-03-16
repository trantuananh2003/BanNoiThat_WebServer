using BanNoiThat.Application.Service.CategoriesService;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.Database
{
    public interface IServiceCategories
    {
        Task CreateCategoryAsync(CreateCategoriesRequest model);
        Task<CategoryResponse> GetCategoryAsync(string id);
        Task<IEnumerable<CategoryResponse>> GetCategoriesForClientAsync();
        Task<IEnumerable<CategoryResponse>> GetCategoriesForAdminAsync();
    }
}
