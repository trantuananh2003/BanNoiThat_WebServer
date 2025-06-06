using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.BrandService;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _dbContext;
        private ICategoriesRepository _categoriesRepository;
        private IProductRepository _productsRepository;
        private IProductItemRepository _productItemsRepository;
        private IBrandsRepository _brandsRepository;
        private ICartRepository _cartRepository;
        private IOrdersRepository _orderRepository;
        private IUserRepository _userRepository;
        private IRolesRepository _rolesRepository;
        private ICouponRepository _couponsRepository;
        private ICouponUsageRepository _couponUsageRepository;
        private ISaleProgramsRepository _saleProgramsRepository;

        public ICategoriesRepository CategoriesRepository => _categoriesRepository = new CategoriesRepository(_dbContext);
        public IProductRepository ProductRepository => _productsRepository = new ProductRepository(_dbContext);
        public IProductItemRepository ProductItemRepository => _productItemsRepository = new ProductItemRepository(_dbContext);
        public IBrandsRepository BrandRepository => _brandsRepository = new BrandRepository(_dbContext);
        public ICartRepository CartRepository => _cartRepository = new CartRepository(_dbContext);
        public IOrdersRepository OrderRepository => _orderRepository = new OrderRepository(_dbContext);
        public IUserRepository UserRepository => _userRepository = new UserRepository(_dbContext);
        public IRolesRepository RolesRepository => _rolesRepository = new RoleRepository(_dbContext);
        public ICouponRepository CouponsRepository => _couponsRepository = new CouponsRepository(_dbContext);
        public ICouponUsageRepository CouponUsageRepository => _couponUsageRepository = new CouponUsageRepository(_dbContext);
        public ISaleProgramsRepository SaleProgramsRepository => _saleProgramsRepository = new SaleProgramsRepository(_dbContext);

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_dbContext != null) _dbContext.Dispose();
        }
    }
}
