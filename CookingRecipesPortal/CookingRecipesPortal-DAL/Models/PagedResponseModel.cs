namespace CookingRecipesPortal_DAL.Models
{
    public class PagedResponseModel<T> where T : class, new()
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public IList<T> Data { get; set; } = new List<T>();
    }
}
