using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Security Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    [Authorize]

    public class SecurityController(IClaimsService ClaimsService, IProfileService ProfileService) : Controller
    {
        /// <summary>
        /// Security method with view
        /// </summary>
        /// <returns></returns>
        public IActionResult IdxSecurity()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

    }
}
