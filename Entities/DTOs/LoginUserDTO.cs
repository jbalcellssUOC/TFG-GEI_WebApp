using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    /// <summary>
    /// LoginUserDTO
    /// </summary>
    public class LoginUserDTO
    {
        [Required]
		[RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
		[EmailAddress]
		public string? Username { get; set; }

        [Required]
		[StringLength(125, MinimumLength = 8)]
		//[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")]
		[DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Name { get; set; }

        [Required]
        public bool KeepSigned { get; set; }

        public string? AuthToken { get; set; }
	}
}
