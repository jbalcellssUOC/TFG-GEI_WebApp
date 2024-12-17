using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for user authentication.
    /// </summary>
    public interface IAuthService
	{
        /// <summary>
        /// Check if the user exists and the password is correct.
        /// </summary>
        /// <param name="loginUserDto">DTO of user login data</param>
        /// <returns>Returns True when authentication is successful. Otherwise returns False</returns>
        public Task<bool> CheckUserAuth(LoginUserDTO loginUserDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        public bool CheckUserExist(LoginUserDTO loginUserDTO);

        /// <summary>
        /// Check if the user is blocked.
        /// </summary>
        /// <param name="loginUserDTO"></param>
        /// <returns></returns>
        public Task<bool> CheckUserIsBlocked(LoginUserDTO loginUserDTO);

        /// <summary>
        /// Icnrease the number of retries of a user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<bool> IncreaseUserRetries(string Username);

        /// <summary>
        /// Reset the number of retries of a user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Task<bool> ResetUserRetries(string Username);

        /// <summary>
        /// Get JWT data from a JWT token.
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public JWTDTO GetJWTData(string jwtToken);

        /// <summary>
        /// Create a new claims identity.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateClaimsIdentity(string UserId);

        /// <summary>
        /// Check if new account can be careated.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public bool CanCreateNewAccount(string Username);

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<bool> CreateNewAccount(string Username, string Name, string Password);

        /// <summary>
        /// Change the password of a user.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<bool> ChangePassword(string Username, string Password);

        /// <summary>
        /// Reset the password of a user with the token sended by email.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<string> UserNewTokenResetPassword(string Username, HttpRequest request);

        /// <summary>
        /// Check if the token is valid.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckUserToken(string token);

        /// <summary>
        /// Create a new security JWT token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<string> CreateJWTSecurityToken(string username, string ApiKeySecret);
    }
}
