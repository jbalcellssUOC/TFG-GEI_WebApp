using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// ClaimsService class
    /// </summary>
    /// <param name="ContextAccessor"></param>
    public class ClaimsService(IHttpContextAccessor ContextAccessor) : IClaimsService
    {
        /// <summary>
        /// Get claim value from user
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        public string GetClaimValue(string ClaimType)
        {
            var ClaimsIdentity = ContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            return ClaimsIdentity!.FindFirst(ClaimType)!.Value;
        }
    }
}
