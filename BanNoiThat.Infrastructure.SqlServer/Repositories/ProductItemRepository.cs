using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class ProductItemRepository: Repository<ProductItem>, IProductItemRepository
    {
        ApplicationDbContext _db;

        public ProductItemRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }
    }
}
