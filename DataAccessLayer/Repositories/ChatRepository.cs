using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Chat repository
    /// </summary>
    /// <param name="bbddcontext"></param>
    public class ChatRepository(BBDDContext bbddcontext) : IChatRepository
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Add chat message to the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userChat"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public async Task<bool> AddChatMessage(string userId, string userChat, bool source, string message, DateTime datetime)
        {
            bool result = false;
            Guid userGuid;
            var user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.Login == userId);
            if (user != null)
            {
                userGuid = user.UserId;
                try
                {
                    AppChat appChat = new()
                    {
                        UserId = userGuid,
                        UserName = userChat,
                        Source = source,
                        Message = message,
                        Datetime = datetime
                    };

                    bbddcontext.Add(appChat);
                    await bbddcontext.SaveChangesAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
                }
            }

            return result;
        }

        /// <summary>
        /// Get all user chat messages
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserChatMessageDTO> GetUserChatMessagesAsync(string userId)
        {
            List<UserChatMessageDTO> messages = [];
            var user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.Login == userId);
            if (user != null)
            {
                var chatMessages = bbddcontext.AppChats.AsNoTracking().Where(AppChats => AppChats.UserId == user.UserId).ToList();
                foreach (var chatMessage in chatMessages)
                {
                    messages.Add(new UserChatMessageDTO
                    {
                        IdxSec = chatMessage.IdxSec,
                        UserName = chatMessage.UserName,
                        Source = chatMessage.Source,
                        Message = chatMessage.Message,
                        Datetime = chatMessage.Datetime
                    });
                }
            }

            return messages;
        }
    }
}
