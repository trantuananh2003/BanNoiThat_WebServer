using BanNoiThat.Application.DTOs.BrandDto;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceBrands
    {
        Task CreateBrandAsync(CreateBrandRequest modelRequest);
        Task DeleteBrandAsync(string id);
        Task<IEnumerable<BrandResponse>> GetAllBrandAsync();
        Task<BrandResponse> GetBrandAsync(string id);
        Task UpdateBrandAsync(string id, UpdateBrandRequest modelRequest);
    }
}
