using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// User repository
    /// </summary>
    /// <param name="bbddcontext"></param>
    public class UserRepository(BBDDContext bbddcontext) : IUserRepository
	{
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public AppUser? GetUserByEmail(string Username)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.Login == Username);
                return user;
            }

            return null;
        }

        /// <summary>
        /// Get user by token
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public AppUser? GetUserByToken(string Token)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.TokenID == Token);
                return user;
            }

            return null;
        }

        /// <summary>
        /// Get user id by email
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Guid GetUserIdByEmail(string Username)
        {
            Guid userId = Guid.Empty;
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                var user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null) userId = user.UserId;
            }

            return userId;
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public DashboardUserProfileDTO GetUserProfile(string Username)
        {
            DashboardUserProfileDTO userProfile = null!;
            var user = bbddcontext.AppUsers.AsNoTracking().FirstOrDefault(AppUser => AppUser.Login == Username);
            if (user != null)
            {
                var usersRoles = bbddcontext.AppUsersRoles.AsNoTracking().FirstOrDefault(AppUsersRoles => AppUsersRoles.UserId == user.UserId);
                if (usersRoles != null)
                {
                    var role = bbddcontext.SysRoles.AsNoTracking().FirstOrDefault(SysRoles => SysRoles.Role == usersRoles!.Role);
                    if (role != null)
                    {
                        DashboardUserProfileDTO dashboardUserProfileDTO = new()
                        {
                            UserLogin = user.Login,
                            UserName = user.Name,
                            UserProfile = role.Description
                        };
                        return dashboardUserProfileDTO;
                    }
                }
            }

            return userProfile!;
        }

        /// <summary>
        /// Increase user login retries
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> IncreaseUserRetries(string Username)
        {
            bool result = false;
            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null){
                    user.Retries++;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Reset user login retries
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> ResetUserRetries(string Username)
        {
            bool result = false;
            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.IsBlocked = false;
                    user.Retries = 0;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> BlockUser(string Username)
        {
            bool result = false;

            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.IsBlocked = true;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccount(string Username, string Name, string Password)
        {
            bool result = false;

            try
            {
                AppUser appUser = new()
                {
                    Login = Username,
                    Password = Password,
                    Name = Name,
                    IsBlocked = false,
                    Retries = 0,
                };

                bbddcontext.Add(appUser);
                await bbddcontext.SaveChangesAsync();

                AppUsersRole appUsersRole = new()
                {
                    UserId = appUser.UserId,
                    Role = "10",
                };

                bbddcontext.Add(appUsersRole);
                await bbddcontext.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Update user token
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserToken(string Username, string newToken)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.TokenID = newToken;
                    user.TokenIssuedUTC = DateTime.UtcNow;
                    user.TokenExpiresUTC = DateTime.UtcNow.AddMinutes(15);
                    user.TokenIsValid = true;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPassword(Guid UserId, string Password)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.UserId == UserId);
                if (user != null)
                {
                    user.IsBlocked = false;
                    user.Retries = 0;
                    user.Password = Password;
                    user.TokenIsValid = false;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add user device data
        /// </summary>
        /// <param name="userDDDTO"></param>
        /// <returns></returns>
        public async Task<bool> AddUserDD(UserDDDTO userDDDTO)
        {
            bool result = false;
            try
            {
                AppUsersStat AppUsersStat = new()
                {
                    UserId = userDDDTO.UserId,
                    SRconnectionId = null,
                    SRconnected = true,
                    IPv4 = userDDDTO.IPAddress,
                    IPv6 = "",
                    Location = userDDDTO.DDCity,
                    DevId = "",
                    DevName = "",
                    DevOS = userDDDTO.DDOs?.Match != null ? $"{userDDDTO.DDOs.Match.Name} {userDDDTO.DDOs.Match.Version} ({userDDDTO.DDOs.Match.ShortName}, {userDDDTO.DDOs.Match.Platform})" : "Unknown OS",
                    DevBrowser = userDDDTO.DDBrowser?.Match != null ? $"{userDDDTO.DDBrowser.Match.Name} ({userDDDTO.DDBrowser.Match.Version}, {userDDDTO.DDBrowser.Match.ShortName}, {userDDDTO.DDBrowser.Match.Type})" : "Unknown Browser",
                    DevBrand = userDDDTO.DDBrand ?? "Unknown Brand",
                    DevBrandName = userDDDTO.DDBrand ?? "Unknown Brand",
                    DevType = userDDDTO.DDtype ?? "Unknown Type",
                    IsoDateC = DateTime.Now,
                    IsoDateM = DateTime.Now,
                };
                await bbddcontext.AddAsync(AppUsersStat);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Get all user stats
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public List<AppUsersStat> GetAllUserStats(string Username)
        {
            Guid UserId = GetUserIdByEmail(Username);
            var usersStats = bbddcontext.AppUsersStats
                .AsNoTracking()
                .Where(u => u.UserId == UserId)
                .OrderByDescending(u => u.Id)
                .ToList();
            return usersStats;
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<AppUser> GetUserDetails(string Username)
        {
            AppUser userResult = null!;
            await Task.Run(() =>
            {
                var user = bbddcontext.AppUsers
                    .AsNoTracking()
                    .Include(u => u.AppLoggers)
                    .Include(u => u.AppProducts)
                    .Include(u => u.AppUsersRoles)
                    .Include(u => u.AppUsersStats)
                    .FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    userResult = user;
                }
            });

            return userResult!;
        }
    }
}
