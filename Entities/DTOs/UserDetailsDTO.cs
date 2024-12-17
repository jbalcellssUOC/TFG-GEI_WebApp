using Entities.Models;

namespace Entities.DTOs
{
    public class UserDetailsDTO
    {
        /// <summary>
        /// UUID unique User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// User login email
        /// </summary>
        public string Login { get; set; } = null!;

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// User surname
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// User phone
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// User address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Internal comments
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// User is admin
        /// </summary>
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// API token
        /// </summary>
        public string? APIToken { get; set; }

        /// <summary>
        /// API Host
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// IPv4 address
        /// </summary>
        public string? IPv4 { get; set; }

        /// <summary>
        /// Location based on IPv4
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Collection of AppLogger
        /// </summary>
        public virtual ICollection<AppLogger> AppLoggers { get; set; } = new List<AppLogger>();

        /// <summary>
        /// Collection of AppProduct
        /// </summary>
        public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();

        /// <summary>
        /// Collection of AppUsersRole
        /// </summary>
        public virtual ICollection<AppUsersRole> AppUsersRoles { get; set; } = new List<AppUsersRole>();

        /// <summary>
        /// Collection of AppUsersStat
        /// </summary>
        public virtual ICollection<AppUsersStat> AppUsersStats { get; set; } = new List<AppUsersStat>();
    }
}
