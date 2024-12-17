using System.ComponentModel.DataAnnotations;

namespace ExternalAPI.POCO
{
    public class ApiUserAuth
    {
        /// <summary>The same username you use to log in to the Codis365 backend..</summary>
        /// <example>john.doe@mycompany.org</example>
        [Required]
        public string? Username { get; set; }
        /// <summary>The same password you use to log in to the Codis365 backend..</summary>
        /// <example>*********</example>
        [Required]
        public string? Password { get; set; }
    }
}
