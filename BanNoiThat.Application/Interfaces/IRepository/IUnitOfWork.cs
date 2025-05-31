using BanNoiThat.Application.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ICouponsRepository CouponsRepository { get; }

        Task SaveChangeAsync();
    }
}
