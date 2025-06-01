using BanNoiThat.Application.Interfaces.IRepository;

namespace BanNoiThat.Application.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; }
        IProductRepository ProductRepository { get; }
        IBrandsRepository BrandRepository { get; }
        ICartRepository CartRepository { get; }
        IOrdersRepository OrderRepository { get; }
        IUserRepository UserRepository { get; }
        IRolesRepository RolesRepository { get; }
        ICouponRepository CouponsRepository { get; }
        ICouponUsageRepository CouponUsageRepository { get; }

        Task SaveChangeAsync();
    }
}
