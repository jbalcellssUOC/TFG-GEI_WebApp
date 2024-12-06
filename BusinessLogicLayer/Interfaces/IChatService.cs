using Entities.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for chat operations
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Adds a new meesage to the chat.    
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userChat"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="datetime"></param>
        /// <returns>Returns true if new chat message is added succesfully to the chat database</returns>
        public Task<bool> AddUserChatMessage(string user, string userChat, bool source, string message, DateTime datetime);

        /// <summary>
        /// Returns all chat messages from a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns a list of all user messages in the database</returns>
        public List<UserChatMessageDTO> GetAllUserChatMessages(string user);
    }
}
