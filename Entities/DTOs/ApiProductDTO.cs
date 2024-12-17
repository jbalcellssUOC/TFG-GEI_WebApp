using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class ApiProductDTO : BodyProductDTO
    {
        /// <summary>
        /// Creation date of the barcode.
        /// </summary>
        [Display(Name = "CreatedOn", Description = "Creation date of the barcode.")]
        public DateTime? CreatedOn { get; set; }
    }
}
