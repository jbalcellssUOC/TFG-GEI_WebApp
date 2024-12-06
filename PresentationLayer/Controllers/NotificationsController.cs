using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Notifications Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    public class NotificationsController(IClaimsService ClaimsService, IProfileService ProfileService): Controller
    {
        /// <summary>
        /// Notifications me with view
        /// </summary>
        /// <returns></returns>
        public IActionResult IdxNotifications()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }
    }
}
