using Entities.DTOs;
using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.POCO
{
    /// <summary>
    /// Gets or sets the API response DTO.
    /// </summary>
    /// <remarks>
    /// ApiResponse constructor.
    /// </remarks>
    /// <param name="success"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    public class ApiResponseProducts(bool success, string message, PaginatedProductList<ApiProductDTO> data)
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        [Required]
        [Display(Name = "Success", Description = "Indicates whether the request was successful.")]
        public bool Success { get; set; } = success;

        /// <summary>
        /// Gets or sets the message about the API response.
        /// </summary>
        [Required]
        [Display(Name = "Message", Description = "Provides a message about the API response.")]
        public string Message { get; set; } = message;

        /// <summary>
        /// Gets or sets the paginated list of dynamic barcodes.
        /// </summary>
        [Required]
        [Display(Name = "Data", Description = "Contains the paginated list of products.")]
        public PaginatedProductList<ApiProductDTO> Data { get; set; } = data!;
    }
}
