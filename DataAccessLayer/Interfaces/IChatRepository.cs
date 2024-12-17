using Entities.DTOs;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Chat repository interface
    /// </summary>
    public interface IChatRepository
    {
        /// <summary>
        /// Add chat message to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userChat"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public Task<bool> AddChatMessage(string user, string userChat, bool source, string message, DateTime datetime);

        /// <summary>
        /// Get all user chat messages
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserChatMessageDTO> GetUserChatMessagesAsync(string userId);
    }
}
