using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using NLog;
using PresentationLayer;
using Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// SignIn Controller
    /// </summary>
    /// <param name="HttpContextAccessor"></param>
    /// <param name="AuthService"></param>
    /// <param name="LocalizeString"></param>
    [Authorize]
    [Route("[controller]/[action]")]
    public class SignInController(
		IHttpContextAccessor HttpContextAccessor, 
		IAuthService AuthService, 
		IStringLocalizer<BasicResources> LocalizeString
        ) : Controller
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Login GET method with view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ModelState.Clear();
            return View("Login", new LoginUserDTO());
        }

        /// <summary>
        /// Login POST method with view
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
		{
			try
			{
                if (ModelState.IsValid || !loginUserDTO.AuthToken.IsNullOrEmpty())
				{
					ModelState.Clear();
                    if (loginUserDTO.AuthToken.IsNullOrEmpty())
                    {
                        if (await AuthService.CheckUserAuth(loginUserDTO))
                        {
                            if (await AuthService.CheckUserIsBlocked(loginUserDTO))
                            {
                                ModelState.Clear();
                                ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR3"]);
                                return View("Login", loginUserDTO);
                            }
                            else
                            {
                                await DoLogin(loginUserDTO.Username!, loginUserDTO.KeepSigned);
                                return RedirectToAction("Index", "Dashboard");
                            }
                        }
                        else
                        {
                            if (AuthService.CheckUserExist(loginUserDTO))
                            {
                                if (await AuthService.CheckUserIsBlocked(loginUserDTO))
                                {
                                    ModelState.Clear();
                                    ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR3"]);
                                    return View("Login", loginUserDTO);
                                }
                                else
                                {
                                    ModelState.Clear();
                                    await AuthService.IncreaseUserRetries(loginUserDTO.Username!);
                                    ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR1"]);
                                    return View("Login", loginUserDTO);
                                }
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR1"]);
                                return View("Login", loginUserDTO);
                            }
                        }
                    }
                    else
                    {
                        if (await AuthService.CheckUserAuth(loginUserDTO))
                        {
                            await DoLogin(loginUserDTO.Username!, loginUserDTO.KeepSigned);
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.Clear();
                            JWTDTO jwtToken = AuthService.GetJWTData(loginUserDTO.AuthToken!);
                            loginUserDTO.Username = jwtToken.Email;
                            loginUserDTO.Name = jwtToken.Name;

                            return RedirectToAction("CreateAccount", loginUserDTO);
                        }
                    }
                }
				else
				{
                    Logger.Error(LocalizeString["LOGIN_ERROR2"]);
                    ModelState.Clear();
					ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
					return View("Login", loginUserDTO);
				}
			}
			catch (Exception ex) {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
                ModelState.Clear();
				ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
				return View("Login", loginUserDTO);
			}
		}

        /// <summary>
        /// DoLogin method
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="KeepSigned"></param>
        /// <returns></returns>
        private async Task<bool> DoLogin(string Username, bool KeepSigned)
        {
            try
            {
                await AuthService.ResetUserRetries(Username!);
                ClaimsIdentity identity = AuthService.CreateClaimsIdentity(Username!);
                identity.AddClaim(new Claim(ClaimTypes.Name, Username!));
                identity.AddClaim(new Claim("UserId", Username!)); 
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = KeepSigned });

                return true;
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return false;
        }

        /// <summary>
        /// Logout method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
		[AllowAnonymous]
        public async Task<IActionResult> Logout()
		{
			// Clear the existing cookie to ensure a clean NEW login process
			if (HttpContextAccessor.HttpContext != null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return RedirectToAction("Index", "Dashboard");
			}
			else return StatusCode(StatusCodes.Status400BadRequest);
		}

        /// <summary>
        /// MakeLogout method
        /// </summary>
        /// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
        public async Task<IActionResult> MakeLogout()
		{
			var authBool = "0";
			if (HttpContextAccessor.HttpContext != null)
			{
				// Clear the existing cookie to ensure a clean NEW login process
				await HttpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                foreach (var cookie in HttpContext.Request.Cookies) { 
                    Response.Cookies.Delete(cookie.Key); 
                }
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                await Task.Run(() => { 
					if (HttpContextAccessor.HttpContext.User.Identity != null)
						if (HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated) { authBool = "1"; } 
				});

                return StatusCode(StatusCodes.Status200OK, authBool);
            }
			else return StatusCode(StatusCodes.Status400BadRequest, authBool);
        }

        /// <summary>
        /// CreateAccount method
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateAccount(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            return View("CreateAccount", loginUserDTO);
        }

        /// <summary>
        /// CreateNewAccount method
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewAccount(LoginUserDTO loginUserDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!AuthService.CanCreateNewAccount(loginUserDTO.Username!))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR1"], loginUserDTO.Username));
                        return View("CreateAccount", loginUserDTO);
                    }
                    else
                    {
                        if (!await AuthService.CreateNewAccount(loginUserDTO.Username!, loginUserDTO.Name!, loginUserDTO.Password!))
                        {
                            ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                            return View("CreateAccount", loginUserDTO);
                        }
                        else
                        {
                            await DoLogin(loginUserDTO.Username!, true);
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                    return View("CreateAccount", loginUserDTO);
                }
            }
            catch (Exception ex) {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
                ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                return View("CreateAccount", loginUserDTO);
            }
        }

        /// <summary>
        /// PasswordRecovery method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRecovery()
        {
            ModelState.Clear();
            return View("PasswordRecovery", new LoginUserDTO());
        }

        /// <summary>
        /// PasswordRecoverySentLink method
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult>PasswordRecoverySentLink(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            string newTokenURL = await AuthService.UserNewTokenResetPassword(loginUserDTO.Username!, HttpContextAccessor.HttpContext!.Request);
            return View("PasswordRecoverySentLink", new LoginUserDTO());
        }

        /// <summary>
        /// PasswordResetError method
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordResetError()
        {
            return View("PasswordResetError");
        }

        /// <summary>
        /// PasswordReset method
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [HttpGet("{userToken}")]
        [AllowAnonymous]
        public IActionResult PasswordReset(string userToken)
        {
            ModelState.Clear();
            bool result = AuthService.CheckUserToken(userToken);
            if (!result)
            {
                return RedirectToAction("PasswordResetError");
            }

            LoginUserDTO loginUserDTO = new()
            {
                AuthToken = userToken,
            };

            return View("PasswordChange", loginUserDTO);
        }

        /// <summary>
        /// PasswordChange method
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordChange(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            bool result = AuthService.CheckUserToken(loginUserDTO.AuthToken!);
            if (!result)
            {
                return RedirectToAction("PasswordResetError");
            }

            result = await AuthService.ChangePassword(loginUserDTO.AuthToken!, loginUserDTO.Password!);
            return View("PasswordChangeOk");
        }
    }
}
