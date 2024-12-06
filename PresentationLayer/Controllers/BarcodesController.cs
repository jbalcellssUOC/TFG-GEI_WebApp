using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Barcodes Controller
    /// </summary>
    /// <param name="ClaimsService"></param>
    /// <param name="ProfileService"></param>
    /// <param name="BarcodeService"></param>
    [Authorize]
    public class BarcodesController(IClaimsService ClaimsService, IProfileService ProfileService, IBarcodeService BarcodeService) : Controller
    {
        public IActionResult StaticBarcodes()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.CodebarList = BarcodeService.GetAllCBStatic(UserProfile.UserLogin!).Take(10);

            return View(UserProfile);
        }

        public IActionResult DynamicBarcodes()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.CodebarList = BarcodeService.GetAllCBDynamic(UserProfile.UserLogin!).Take(10);

            return View(UserProfile);
        }

        public IActionResult Management()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            return View(UserProfile);
        }
    }
}
