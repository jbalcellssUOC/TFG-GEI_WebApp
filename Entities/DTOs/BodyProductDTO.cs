using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    /// <summary>
    /// Data transfer object for adding a new product.
    /// </summary>
    public class BodyProductDTO 
    {
        /// <summary>
        /// Product reference
        /// </summary>
        /// <example>REF001</example>
        public string? Reference { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        /// <example>Fashion</example>
        public string? Category { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        /// <example>Experience elegance with the Velvet Luxe Handbag, featuring plush velvet fabric and gold-tone accents, perfect for adding a touch of sophistication to any outfit.</example>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        /// <example>19,50 €</example>
        public decimal? Price { get; set; }

        /// <summary>
        /// Barcode type
        /// </summary>
        /// <example>QR</example>
        public string? CBType { get; set; }

        /// <summary>
        /// Barcode value
        /// </summary>
        /// <example>https://www.uoc.edu/barcode.fashion.ref001</example>
        public string? CBValue { get; set; }

        /// <summary>
        /// Barcode shortlink
        /// </summary>
        /// <example>G65DSS</example>
        public string? CBShortLink { get; set; }

        /// <summary>
        /// Product ActionType
        /// </summary>
        /// <example>Link</example>
        public string? ActionId { get; set; }
    }
}
