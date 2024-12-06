using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Integration Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    [Authorize]

    public class IntegrationController(IClaimsService ClaimsService, IProfileService ProfileService) : Controller
    {
        /// <summary>
        /// Products
        /// </summary>
        /// <returns></returns>
        public IActionResult Products()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

        /// <summary>
        /// Prices 
        /// </summary>
        /// <returns></returns>
        public IActionResult Prices()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

        /// <summary>
        /// Rates
        /// </summary>
        /// <returns></returns>
        public IActionResult Rates()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

    }
}
