using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// User Service class
    /// </summary>
    /// <param name="UserRepository"></param>
    /// <param name="mapper"></param>
    public class UserService(
        IUserRepository UserRepository,
        IMapper Mapper,
        IHttpContextAccessor HttpContextAccessor
        ) : IUserService
    {
        /// <summary>
        /// Get all users method
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public List<AppUsersStat> GetAllUserStats(string Username) { 
            return UserRepository.GetAllUserStats(Username);
        }

        /// <summary>
        /// Get user details method
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<UserDetailsDTO> GetUserDetails(string Username)
        {
            UserDetailsDTO userDetailsDTO = null!;
            AppUser appUser = await UserRepository.GetUserDetails(Username);
            if (appUser != null)
            {
                userDetailsDTO = Mapper.Map<UserDetailsDTO>(appUser);    // Map to DTO
                var lastUserStat = userDetailsDTO.AppUsersStats.OrderByDescending(stat => stat.IsoDateC).FirstOrDefault();
                var lastLocation = lastUserStat?.Location;
                var lastIPv4 = lastUserStat?.IPv4;
                userDetailsDTO.Host = HttpContextAccessor.HttpContext?.Request.Host.Value;
                userDetailsDTO.Location = lastLocation;
                userDetailsDTO.IPv4 = lastIPv4;
            }
            return userDetailsDTO;
        }
    }
}
