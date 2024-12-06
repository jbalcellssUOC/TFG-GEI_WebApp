using Entities.DTOs;
using Entities.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Entities.POCO
{

    /// <summary>
    /// Represents the API response DTO for static barcodes.
    /// </summary>
    [SwaggerSchema("API response for static barcodes")]
    public class ApiResponseStaticBarcodes
    {
        public ApiResponseStaticBarcodes(bool success, string message, PaginatedStaticBarcodeList<ApiBCStaticDTO> data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Value indicating whether the request was successful.
        /// </summary>
        [Required]
        [Display(Name = "Success", Description = "Indicates whether the request was successful.")]
        public bool Success { get; set; }

        /// <summary>
        /// A message description about the API response.
        /// </summary>
        /// <example>All user static barcodes have been retrieved succesfully.</example>
        [Required]
        [Display(Name = "Message", Description = "Provides a message about the API response.")]
        public string Message { get; set; }

        /// <summary>
        /// A paginated list of dynamic barcodes.
        /// </summary>
        /// <remarks>Contains the paginated list of dynamic barcodes.</remarks>
        [Required]
        [Display(Name = "Data", Description = "Contains the paginated list of dynamic barcodes.")]
        [SwaggerSchema("Contains the paginated list of dynamic barcodes.", Format = "PaginatedStaticBarcodeList<AppCBStatic>")]
        public PaginatedStaticBarcodeList<ApiBCStaticDTO> Data { get; set; }
    }
}
