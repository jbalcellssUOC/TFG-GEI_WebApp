using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// ProfileService class
    /// </summary>
    /// <param name="UserRepository"></param>
    public class ProfileService(IUserRepository UserRepository) : IProfileService
    {
        /// <summary>
        /// Get user profile method
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DashboardUserProfileDTO GetUserProfile(string username)
        {
            DashboardUserProfileDTO userProfile = UserRepository.GetUserProfile(username);
            return userProfile;
        }
    }
}
