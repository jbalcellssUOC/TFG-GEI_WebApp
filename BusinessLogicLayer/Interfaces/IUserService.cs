using Entities.DTOs;
using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface of the Service for user operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all user stats
        /// </summary>
        /// <returns></returns>
        public List<AppUsersStat> GetAllUserStats(string Username);

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<UserDetailsDTO> GetUserDetails(string Username);
    }

}
