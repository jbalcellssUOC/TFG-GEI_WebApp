using Entities.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for encryption operations
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Checks if the token is valid and returns the token
        /// </summary>
        /// <param name="jwtTokenString"></param>
        /// <returns></returns>
        public JwtSecurityToken JWT_CheckExternalToken(string jwtTokenString);

        /// <summary>
        /// Gets the email from the token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public string JWT_GetEmailFromToken(JwtSecurityToken jwtToken);

        /// <summary>
        /// Gets the data from the token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public JWTDTO JWT_GetDataFromToken(JwtSecurityToken jwtToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public bool JWT_IsExpired(JwtSecurityToken jwtToken);

        /// <summary>
        /// Generates a JWT token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string JWT_Generate(Guid userId);

        /// <summary>
        /// Validates the JWT token
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public SecurityToken? JWT_Validate(string jwt);

        /// <summary>
        /// Checks if the password is correct
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool BCrypt_CheckPassword(string password, string hash);

        /// <summary>
        /// Encrypts the password with bcrypt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string BCrypt_EncryptPassword(string password);
    }
}
