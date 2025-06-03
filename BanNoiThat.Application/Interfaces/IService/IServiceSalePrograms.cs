using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceSalePrograms
    {
        Task ApplySaleProgramsToProduct(SaleProgram modelSale);
        Task GetBackPrice(string modelSaleProgramId);
        Task PutBackPrice(string modelSaleProgramId);
    }
}
