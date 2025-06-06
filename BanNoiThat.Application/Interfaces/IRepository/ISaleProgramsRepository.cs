using BanNoiThat.Application.DTOs.SaleProgramDtos;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IRepository
{
    public interface ISaleProgramsRepository : IRepository<SaleProgram>
    {
        Task<SaleProgramIncludeProductsDto> GetSaleProgramsWithUniqueProducts();
    }
}
