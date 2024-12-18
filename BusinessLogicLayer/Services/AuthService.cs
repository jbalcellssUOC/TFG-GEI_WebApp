using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// AuthService class implements IAuthService interface
    /// </summary>
    /// <param name="UserRepository"></param>
    /// <param name="Encryption"></param>
    /// <param name="UserDDService"></param>
    /// <param name="NotificationService"></param>
    /// <param name="EmailBodyHelper"></param>
    public class AuthService(
        IUserRepository UserRepository, 
        IEncryptionService Encryption, 
        IUserDDService UserDDService,
        INotificationService NotificationService,
        IHelpersService EmailBodyHelper
        ) : IAuthService{

        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();     // NLog Logger

        /// <summary>
        /// CheckUserAuth method checks if the user is authenticated
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserAuth(LoginUserDTO loginUserDTO)
		{
            if (!string.IsNullOrEmpty(loginUserDTO.AuthToken))
            {
                bool result = false;

                // Check user by JWT and OAuth2 Token from Google Auth Services
                JwtSecurityToken jwtToken = Encryption.JWT_CheckExternalToken(loginUserDTO.AuthToken!);
                if (jwtToken != null)
                {
                    string email = Encryption.JWT_GetEmailFromToken(jwtToken);
                    AppUser? user = UserRepository.GetUserByEmail(email);
                    if (user != null && !Encryption.JWT_IsExpired(jwtToken))
                    {
                        await UserDDService.AddUserDeviceDetector(user.Login);
                        loginUserDTO.Username = email;
                        result = true;
                    }
                }

                return result;
            }
			else
			{
                // Check user by username & password
                AppUser? user = UserRepository.GetUserByEmail(loginUserDTO.Username!);
                if (user != null)
                {
                    if (Encryption.BCrypt_CheckPassword(loginUserDTO.Password!, user.Password!))
                    {
                        await UserDDService.AddUserDeviceDetector(user.Login);
                        return true;
                    }
                }
            }

			return false;
		}

        /// <summary>
        /// Check
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserIsBlocked(LoginUserDTO loginUserDTO)
        {
            // Check if user is blocked
            bool result = true;
            AppUser? user = UserRepository.GetUserByEmail(loginUserDTO.Username!);
            if (user != null)
            {
                if (!(user.IsBlocked ?? true))
                {
                    if (user.Retries > 2)
                    {
                        await UserRepository.BlockUser(loginUserDTO.Username!);
                        result = true;
                    }
                    else result = false;
                }
                else result = true;
            }

            return result;
        }

        /// <summary>
        /// Increase login user retries
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> IncreaseUserRetries(string Username)
        {
            bool result = await UserRepository.IncreaseUserRetries(Username);
            return result;
        }

        /// <summary>
        /// Reset login user retries
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> ResetUserRetries(string Username)
        {
            bool result = await UserRepository.ResetUserRetries(Username);
            return result;
        }

        /// <summary>
        /// Check if user exist
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        public bool CheckUserExist(LoginUserDTO loginUserDTO)
        {
            // Check if user exist
            AppUser? user = UserRepository.GetUserByEmail(loginUserDTO.Username!);
            return user != null;
        }

        /// <summary>
        /// Get user data in JWT Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JWTDTO GetJWTData(string token)
        {
            JWTDTO? JWT = null;
            try
            {
                JwtSecurityToken jwtToken = Encryption.JWT_CheckExternalToken(token);
                JWT = Encryption.JWT_GetDataFromToken(jwtToken);
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return JWT!;
        }

        /// <summary>
        /// Create Claims Identity
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateClaimsIdentity(string UserId)
        {
            ClaimsIdentity? identity = null;
            try
            {
                identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, UserId));
                identity.AddClaim(new Claim("UserId", UserId));
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return identity!;
        }

        /// <summary>
        /// Get if user can create an account
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public bool CanCreateNewAccount(string Username)
        {
            AppUser? user = UserRepository.GetUserByEmail(Username);
            return (user == null);
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<bool> CreateNewAccount(string Username, string Name, string Password)
        {
            bool result = false;
            try
            {
                string EncryptedPassword = Encryption.BCrypt_EncryptPassword(Password);
                result = await UserRepository.CreateAccount(Username, Name, EncryptedPassword);
                if (result) {
                    StringBuilder bodyHtml = EmailBodyHelper.EmailBodyAccount_Welcome(Username, Name);
                    await NotificationService.EmailNotification("Codis365 Support", Username, "", "Welcome to Codis365!", bodyHtml.ToString(), "");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Create a link with a token to reset user password
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> UserNewTokenResetPassword(string Username, HttpRequest request)
        {
            try
            {
                var newToken = GetNewTokenResetPassword();
                var newTokenURL = GetNewURLTokenResetPassword(newToken, request);
                await UserRepository.UpdateUserToken(Username, newToken);

                StringBuilder bodyHtml = EmailBodyHelper.EmailBodyPassword_New(Username, newTokenURL);
                await NotificationService.EmailNotification("Codis365 Support", Username, "", "Codis365 Password recovery", bodyHtml.ToString(), "");

                return newTokenURL;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return "";
        }

        /// <summary>
        /// Check if the user token is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckUserToken(string token)
        {
            try
            {
                AppUser? user = UserRepository.GetUserByToken(token!);
                if (user != null)
                {
                    if (user.TokenIsValid == true && DateTime.UtcNow <= user.TokenExpiresUTC)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return false;
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="AuthToken"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(string AuthToken, string Password)
        {
            try
            {
                AppUser? user = UserRepository.GetUserByToken(AuthToken);
                if (user != null)
                {
                    bool result = await UserRepository.UpdateUserPassword(user.UserId, Encryption.BCrypt_EncryptPassword(Password));
                    if (result)
                    {
                        StringBuilder bodyHtml = EmailBodyHelper.EmailBodyPassword_Change(user.Login);
                        await NotificationService.EmailNotification("Codis365 Support", user.Login, "", "Codis365 Password changed", bodyHtml.ToString(), "");

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return false;
        }

        /// <summary>
        /// Create a JWT Security Token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<string> CreateJWTSecurityToken(string username, string ApiKeySecret)
        {
            string securityToken = "";                              // Serialized security Token
            var expTime = 15;                                       // 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();       // Token Handle

            await Task.Run(() =>
            {
                try {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, username)]),   // User Name
                        Expires = (Debugger.IsAttached) ? 
                                    DateTime.UtcNow.AddMinutes(9999).ToLocalTime() :            // Expiration Time for debugger and tests users   
                                    DateTime.UtcNow.AddMinutes(expTime).ToLocalTime(),          // Expiration Time
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiKeySecret)),    // Symetric key  
                            SecurityAlgorithms.HmacSha256Signature                              // Algorithm 
                         )
                    };
                    var JWTSecurityToken = tokenHandler.CreateToken(tokenDescriptor);           // Create Token  
                    securityToken = tokenHandler.WriteToken(JWTSecurityToken);                  // Serialize Token
                }
                catch (Exception ex) 
                {
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
                }
            });

            return securityToken;
        }


        //############################################################################################################//
        //############################################## PRIVATE METHODS ##############################################//
        //############################################################################################################//

        /// <summary>
        /// Get a new token to reset user password
        /// </summary>
        /// <returns></returns>
        private static string GetNewTokenResetPassword()
        {
            var newToken = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n");
            return newToken;
        }

        /// <summary>
        /// Get a new URL with the token to reset user password
        /// </summary>
        /// <param name="newToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetNewURLTokenResetPassword(string newToken, HttpRequest request)
        {
            string newTokenURL = "";
            try
            {
                var rawurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
                var uri = new Uri(rawurl);
                newTokenURL = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped) + "/SignIn/PasswordReset/" + WebUtility.UrlEncode(newToken);
                return newTokenURL;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return newTokenURL;
        }
    }
}
