using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class ApiBCDynamicDTO
    {
        /// <summary>
        /// Unique ID of the barcode.
        /// </summary>
        [Required]
        [Display(Name = "ID", Description = "Barcode unique Id.")]
        /// <example>e0f31b89-5c5b-4d48-8f4f-6e72d78c2b18</example>
        public Guid BarcodeId { get; set; }

        /// <summary>
        /// Barcode description
        /// </summary>
        /// <example>Sample QR Code</example>
        [Display(Name = "Description", Description = "The description of the product.")]
        public string? Description { get; set; }

        /// <summary>
        /// Barcode Type
        /// </summary>
        /// <example>QR</example>
        [Display(Name = "CB Type", Description = "The type of the barcode.")]
        public string? CBType { get; set; }

        /// <summary>
        /// Barcode Value
        /// </summary>
        /// <example>https://www.google.es/barcodes</example>
        [Display(Name = "CB Value", Description = "The value of the barcode.")]
        public string? CBValue { get; set; }

        /// <summary>
        /// Barcode Short link
        /// </summary>
        /// <example>B3HDG4</example>
        [Display(Name = "Cb Short Link", Description = "Short link")]
        public string? CBShortLink { get; set; }


        /// <summary>
        /// Creation date of the barcode.
        /// </summary>
        [Display(Name = "CreatedOn", Description = "Creation date of the barcode.")]
        public DateTime? CreatedOn { get; set; }
    }
}
