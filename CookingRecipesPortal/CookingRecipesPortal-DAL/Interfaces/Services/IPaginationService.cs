using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IPaginationService<T, U> where T : class, new()
    {
        Task<PagedResponseModel<T>> GetPagedResponseAsync(
            PaginationFilter paginationFilter,
            Expression<Func<U, bool>>? filter = null);

        int GetRoundedTotalPages(int totalRecords, int pageSize);
    }
}
