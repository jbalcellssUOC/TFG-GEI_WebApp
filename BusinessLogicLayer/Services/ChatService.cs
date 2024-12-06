using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// ChatService class
    /// </summary>
    /// <param name="ChatRepository"></param>
    public class ChatService (IChatRepository ChatRepository) : IChatService
    {
        /// <summary>
        /// Add user message to chat to database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userChat"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public async Task<bool> AddUserChatMessage(string user, string userChat, bool source, string message, DateTime datetime)
        {
            bool result = await ChatRepository.AddChatMessage(user, userChat, source, message, datetime); 
            return result;
        }

        /// <summary>
        /// Get all user chat messages from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserChatMessageDTO> GetAllUserChatMessages(string user)
        {
            var messages = ChatRepository.GetUserChatMessagesAsync(user);
            return messages;
        }
    }
}
