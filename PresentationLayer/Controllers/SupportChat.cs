using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Support Chat Controller
    /// </summary>
    /// <param name="ContextAccessor"></param>
    /// <param name="ChatService"></param>
    [Authorize]
    public class SupportChat(IHttpContextAccessor ContextAccessor, IChatService ChatService) : Controller
    {
        /// <summary>
        /// Get all user chat messages
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAllUserChatMessages()
        {
            var ClaimsIdentity = ContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            var Claim_UserId = ClaimsIdentity!.FindFirst("UserId")!.Value;
            var messages = ChatService.GetAllUserChatMessages(Claim_UserId);

            return Ok(messages);
        }
    }
}
