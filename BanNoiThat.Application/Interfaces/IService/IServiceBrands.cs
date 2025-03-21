using BanNoiThat.Application.DTOs.Brand;
using BanNoiThat.Application.Service.BrandService;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceBrands
    {
        Task CreateBrandAsync(CreateBrandRequest modelRequest);
        Task<IEnumerable<BrandResponse>> GetAllBrandAsync();
        Task<BrandResponse> GetBrandAsync(string id);
        Task UpdateBrandAsync(string id, UpdateBrandRequest modelRequest);
    }
}
