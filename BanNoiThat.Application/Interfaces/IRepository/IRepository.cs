using System.Linq.Expressions;

namespace BanNoiThat.Application.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = false, string? includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = false,  string? includeProperties = null);
        void AttachEntity(T entity);
        Task DeleteEntityHard(string id);
    }
}
