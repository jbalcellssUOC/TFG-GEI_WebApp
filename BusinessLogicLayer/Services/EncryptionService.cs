using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// EncryptionService class
    /// </summary>
    /// <param name="Configuration"></param>
    public class EncryptionService(IConfiguration Configuration) : IEncryptionService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Check external JWT token
        /// </summary>
        /// <param name="jwtTokenString"></param>
        /// <returns></returns>
        public JwtSecurityToken JWT_CheckExternalToken(string jwtTokenString)
        {
            JwtSecurityToken? jwtToken = null;
            try
            {
                var token = jwtTokenString;
                var handler = new JwtSecurityTokenHandler();
                jwtToken = handler.ReadJwtToken(token);
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return jwtToken!;
        }

        /// <summary>
        /// Get user email from JWT token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public string JWT_GetEmailFromToken(JwtSecurityToken jwtToken)
        {
            string email = "";
            try
            {
                email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value!;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return email;
        }

        /// <summary>
        /// Get user data from JWT token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public JWTDTO JWT_GetDataFromToken(JwtSecurityToken jwtToken)
        {
            JWTDTO? JWT = new();
            try
            {
                JWT.Name = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value!;
                JWT.Email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value!;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return JWT!;
        }

        /// <summary>
        /// Get if JWT token is expired
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public bool JWT_IsExpired(JwtSecurityToken jwtToken)
        {
            bool IsExpiredJWT = true;

            try
            {
                var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;
                var expiryDateUnix = long.Parse(expClaim!);
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var expiryDate = epoch.AddSeconds(expiryDateUnix);
                IsExpiredJWT = expiryDate <= DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return IsExpiredJWT;
        }

        /// <summary>
        /// Generate JWT token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string JWT_Generate(Guid userId)
        {
            return JWT_GetGenerate(userId);
        }

        /// <summary>
        /// Validate JWT token
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public SecurityToken? JWT_Validate(string jwt)
        {
            return JWT_GetValidate(jwt);
        }

        /// <summary>
        /// Check if password is correct with BCrypt Algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool BCrypt_CheckPassword(string password, string hash)
        {
            bool result = false;
            try
            {
                result = Verify(password, hash);
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return result;
        }

        /// <summary>
        /// Encrypt password with BCrypt Algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string BCrypt_EncryptPassword(string password)
        {
            string newPassword = "";
            try
            {
                newPassword = HashPassword(password);
            }
            catch { }   

            return newPassword;
        }

        //############################################################################################################//
        //############################################## PRIVATE METHODS ##############################################//
        //############################################################################################################//

        #region JWTToken Metohds

        /// <summary>
        /// Generate a new JWT token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string JWT_GetGenerate(Guid userId)
        {
            string newToken = "";
            try
            {
                string? secretKey = Configuration["Encryption:SecretKey"];
                if (secretKey == null)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "Codis365",
                        audience: "userAuth",
                        claims: [
                            new Claim("userpasswordlink", userId.ToString())
                        ],
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);
                    newToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();
                }
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return newToken;
        }

        /// <summary>
        /// Validate a JWT token
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        private SecurityToken? JWT_GetValidate(string jwt)
        {
            SecurityToken? securityToken = null;
            try
            {
                string secretKey = Configuration["Encryption:SecretKey"]!;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwt, tokenValidationParameters, out securityToken);
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return securityToken!;
        }

        #endregion
    }
}
