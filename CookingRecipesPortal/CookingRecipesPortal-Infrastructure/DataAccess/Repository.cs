using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CookingRecipesPortal_Infrastructure.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly CookingRecipesPortalContext DbContext;

        public Repository(CookingRecipesPortalContext context)
        {
            DbContext = context;
        }

        public Task<T> AddAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return Task.FromResult(entity);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> RemoveAsync(Guid id)
        {
            var entityToBeDeleted = await DbContext.Set<T>().FindAsync(id);
            if (entityToBeDeleted == null)
            {
                return entityToBeDeleted;
            }
            DbContext.Set<T>().Remove(entityToBeDeleted);

            return entityToBeDeleted;
        }

        public Task<T> RemoveAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);

            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            DbContext.Set<T>().Update(entity);

            return Task.FromResult(entity);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            var entities = DbContext.Set<T>().Where(filter);

            return Task.FromResult(entities.Any());
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(DbContext.Set<T>().Find(id));
        }

        public Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            var entities = DbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            if (skip != null)
            {
                entities = entities.Skip(skip.Value);
            }

            if (take != null)
            {
                entities = entities.Take(take.Value);
            }

            entities = GetEntitiesWithIncludedProperties(entities, includeProperties);

            return Task.FromResult(entities);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> entities = DbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            entities = GetEntitiesWithIncludedProperties(entities, includeProperties);

            return await entities.FirstOrDefaultAsync();
        }

        private static IQueryable<T> GetEntitiesWithIncludedProperties(IQueryable<T> entities, string? includeProperties)
        {
            if (includeProperties != null)
            {
                var propertiesToBeIncluded = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in propertiesToBeIncluded)
                {
                    entities = entities.Include(property);
                }
            }

            return entities;
        }

        public Task<int> GetTotalRecordsAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return Task.FromResult(DbContext.Set<T>().Where(filter).Count());
            }

            return Task.FromResult(DbContext.Set<T>().Count());
        }
    }
}
