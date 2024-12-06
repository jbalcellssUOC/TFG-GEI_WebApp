namespace Entities.Models
{
    /// <summary>
    /// get the list of items, the current page index, and the total number of pages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="pageIndex"></param>
    /// <param name="totalPages"></param>
    public class PaginatedProductList<T>
    {
        public PaginatedProductList(List<T> items, int pageIndex, int totalPages)
        {
            Items = items ?? new List<T>();
            PageIndex = items!.Count > 0 ? pageIndex : 0;
            TotalPages = totalPages;
        }

        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
