using System.ComponentModel.DataAnnotations;

namespace ExternalAPI.DTO
{
    /// <summary>
    /// Data transfer object for adding a static barcode.
    /// </summary>
    public class BodyBCStaticDTO
    {
        /// <summary>Product description</summary>
        /// <example>QR Code for product REF001</example>
        [Display(Name = "Description", Description = "The description of the product.")]
        [Required]
        public string? Description { get; set; }

        /// <summary>Barcode Type</summary>
        /// <example>QR</example>
        [Display(Name = "CB Type", Description = "The type of the barcode.")]
        [Required]
        public string? CBType { get; set; }

        /// <summary>Barcode Value</summary>
        /// <example>https://www.uoc.edu/barcode238478774</example>
        [Display(Name = "CB Value", Description = "The value of the barcode.")]
        [Required]
        public string? CBValue { get; set; }
    }
}
