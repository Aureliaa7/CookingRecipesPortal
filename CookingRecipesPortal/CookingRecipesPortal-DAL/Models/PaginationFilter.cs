namespace CookingRecipesPortal_DAL.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }


        public PaginationFilter()
        {
            PageNumber = Constants.DefaultPageNumber;
            PageSize = Constants.DefaultPageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = (pageNumber < Constants.DefaultPageNumber || pageNumber <= 0) ? Constants.DefaultPageNumber : pageNumber;
            PageSize = (pageSize > Constants.DefaultPageSize || pageSize <= 0) ? Constants.DefaultPageSize : pageSize;
        }
    }
}
