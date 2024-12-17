using Entities.DTOs;
using Entities.Models;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
	{
        /// <summary>
        /// Locate a user by email in the database.
        /// </summary>
        /// <param name="Username">The user email. </param>
        /// <returns>An model class product that has been located. Otherwise null.</returns>
        public AppUser? GetUserByEmail(string Username);

        /// <summary>
        /// Locate a user by email in the database.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public AppUser? GetUserByToken(string Token);

        /// <summary>
        /// Locate a user by email in the database.
        /// </summary>
        /// <param name="Username">The user email. </param>
        /// <returns>The Guid of the user located. Otherwise empty GUID.</returns>
        public Guid GetUserIdByEmail(string Username);

        /// <summary>
        /// Icrea user login retries    
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<bool> IncreaseUserRetries(string Username);

        /// <summary>
        /// Reset user login retries
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<bool> ResetUserRetries(string Username);

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<bool> BlockUser(string Username);

        /// <summary>
        /// Adds user device data to the database
        /// </summary>
        /// <param name="userDDDTO">The user device data DTO. </param>
        /// <returns>Object of type Task asynchronous with the number of insertions.</returns>
        public Task<bool> AddUserDD(UserDDDTO userDDDTO);

        /// <summary>
        /// Updates user token in database
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserToken(string Username, string newToken);

        /// <summary>
        /// Updates user password in database
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserPassword(Guid UserId, string Password);

        /// <summary>
        /// Create a new user account
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<bool> CreateAccount(string Username, string Name, string Password);

        /// <summary>
        /// Get user profile data
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public DashboardUserProfileDTO GetUserProfile(string Username);

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
        public Task<AppUser> GetUserDetails(string Username);
    }
}
