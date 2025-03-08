using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class CategoriesRepository : Repository<Category>, ICategoriesRepository
    {

        public CategoriesRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
