using BusinessLogicLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Analytics Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    /// <param name="UserService"></param>
    [Authorize]
    public class AnalyticsController(IClaimsService ClaimsService, IProfileService ProfileService, IUserService UserService) : Controller
    {
        public IActionResult Index()
        {
            var Username = ClaimsService.GetClaimValue("UserId")!;
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.UserStats = UserService.GetAllUserStats(Username).Take(5);
            
            return View(UserProfile);
        }
    }
}
