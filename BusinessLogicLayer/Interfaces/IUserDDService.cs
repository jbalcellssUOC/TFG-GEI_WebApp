using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface of the Service for user device detector operations
    /// </summary>
    public interface IUserDDService
    {
        /// <summary>
        /// Retrieves non-private device information from which a user logs in and adds it to the user logger database.
        /// </summary>
        /// <param name="userId">Username value as a string.</param>
        /// <returns></returns>
        public Task<bool> AddUserDeviceDetector(string? userId);
    }
}
