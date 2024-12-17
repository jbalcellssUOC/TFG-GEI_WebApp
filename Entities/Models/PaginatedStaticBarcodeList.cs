using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// get the list of items, the current page index, and the total number of pages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="pageIndex"></param>
    /// <param name="totalPages"></param>
    public class PaginatedStaticBarcodeList<T>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedStaticBarcodeList{T}"/> class.
        /// </summary>
        /// <param name="items">The list of items.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="totalPages">The total number of pages.</param>
        public PaginatedStaticBarcodeList(List<T> items, int pageIndex, int totalPages)
        {
            Items = items ?? new List<T>();
            PageIndex = items!.Count > 0 ? pageIndex : 0;
            TotalPages = totalPages;
        }

        /// <summary>
        /// Gets the list of items.
        /// </summary>
        /// <remarks>The list of items.</remarks>
        [Required]
        [Display(Name = "Items", Description = "The list of items.")]
        public List<T> Items { get; }

        /// <summary>
        /// Gets the current page index.
        /// </summary>
        /// <example>1</example>
        [Required]
        [Display(Name = "Page Index", Description = "The current page index.")]
        public int PageIndex { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        /// <example>1</example>
        [Required]
        [Display(Name = "Total Pages", Description = "The total number of pages.")]
        public int TotalPages { get; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        /// <example>false</example>
        [Display(Name = "Has Previous Page", Description = "Indicates whether there is a previous page.")]
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        /// <example>false</example>
        [Display(Name = "Has Next Page", Description = "Indicates whether there is a next page.")]
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
