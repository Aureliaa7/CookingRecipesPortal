using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public abstract class PaginationServiceBase<T, U> : IPaginationService<T, U> where T : class, new()
    {
        public abstract Task<PagedResponseModel<T>> GetPagedResponseAsync(
            PaginationFilter paginationFilter,
            Expression<Func<U, bool>>? filter = null);

        public int GetRoundedTotalPages(int totalRecords, int pageSize)
        {
            var totalPages = ((double)totalRecords / pageSize);
            return Convert.ToInt32(Math.Ceiling(totalPages));
        }

        protected PagedResponseModel<T> GetPagedResponseModel(IList<T> data, int totalRecords, PaginationFilter paginationFilter)
        {
            int roundedTotalPages = GetRoundedTotalPages(totalRecords, paginationFilter.PageSize);
            return new PagedResponseModel<T>
            {
                Data = data,
                TotalPages = roundedTotalPages,
                PageNumber = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
