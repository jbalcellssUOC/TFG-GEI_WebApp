using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Globalization;
using System.Security.Claims;

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

        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Settings method with view
        /// </summary>
        /// <returns></returns>
        public IActionResult IdxSettings()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

        [AllowAnonymous]
        public IActionResult SetLanguage(string cultureUI, string returnUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Url.Content("~/Login");
                }

                var LocaleTmp = $"{cultureUI}-{cultureUI.ToUpper()}";

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(LocaleTmp, cultureUI.Trim())),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Path = "/",
                        HttpOnly = false,
                        SameSite = SameSiteMode.Strict
                    });

                var culture = new CultureInfo(LocaleTmp);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            catch (Exception e)
            {
                Logger.Error($"[M]: {e.Message} [StackT]: {e.StackTrace} [HLink]: {e.HelpLink} [HResult]: {e.HResult} [Source]: {e.Source}");
            }

            return Redirect(returnUrl);
        }
    }
}
