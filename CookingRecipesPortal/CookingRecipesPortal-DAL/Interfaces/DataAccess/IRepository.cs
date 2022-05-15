using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Interfaces.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<T> AddAsync(T entity);

        Task<T?> RemoveAsync(Guid id);

        Task<T> RemoveAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

        Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        Task<int> GetTotalRecordsAsync(Expression<Func<T, bool>>? filter = null);
    }
}
