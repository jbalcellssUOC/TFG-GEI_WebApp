using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Settings Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    [Authorize]

    public class SettingsController(IClaimsService ClaimsService, IProfileService ProfileService) : Controller
    {
        /// <summary>
        /// Settings method with view
        /// </summary>
        /// <returns></returns>
        public IActionResult IdxSettings()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

    }
}
