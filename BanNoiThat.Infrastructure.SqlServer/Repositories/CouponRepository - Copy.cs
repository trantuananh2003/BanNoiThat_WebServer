using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class SaleProgramsRepository : Repository<SaleProgram>, ISaleProgramsRepository
    {
        public SaleProgramsRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
