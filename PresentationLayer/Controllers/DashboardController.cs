using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Dashboard Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    /// <param name="UserService"></param>
    [Authorize]
    public class DashboardController(IClaimsService ClaimsService, IProfileService ProfileService, IUserService UserService) : Controller
    {
        public IActionResult Index()
        {
            var Username = ClaimsService.GetClaimValue("UserId")!;
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(Username);
            ViewBag.UserStats = UserService.GetAllUserStats(Username).Take(5);

            return View(UserProfile);
        }
	}
}
