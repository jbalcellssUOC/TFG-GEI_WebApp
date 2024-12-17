using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Support Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    [Authorize]
    public class SupportController(IClaimsService ClaimsService, IProfileService ProfileService) : Controller
    {
        /// <summary>
        /// User support method with view
        /// </summary>
        /// <returns></returns>
        public IActionResult IdxSupport()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }
    }
}
