using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceSalePrograms
    {
        Task ApplySaleProgramsToProduct(string modesaleProgramId);
        Task GetBackPrice(string modelSaleProgramId);
        Task PutBackPrice(string modelSaleProgramId);
        Task SetNullProgramSale(string modelSaleProgramId);
    }
}
