using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using Entities.DTOs;
using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Diagnostics;
using System.Text;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// UserDDService class
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="userRepository"></param>
    public class UserDDService(
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        INotificationService? NotificationService
            ) : IUserDDService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();
        private DatabaseReader? IpReader = null;
        private string? pathToDB;
        private readonly UserDDDTO userDDDTO = new();

        /// <summary>
        /// Add User Device Detector
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> AddUserDeviceDetector(string? userId)
        {
            UserDDDTO userDDDTO = GetUserDeviceDetector(userRepository.GetUserIdByEmail(userId!));

            if (!Debugger.IsAttached && (userDDDTO.DDCity != "Local" && !userDDDTO.DDCity!.Trim().IsNullOrEmpty()))
            {
                StringBuilder body = new();
                body.Append("<br>Username: " + userId);
                body.Append("<br>Location: " + userDDDTO.DDCity);
                body.Append("<br>IPv4: " + userDDDTO.IPAddress);
                body.Append("<br>OS: " + userDDDTO.DDOs);
                body.Append("<br>Agent: " + userDDDTO.DDBrowser);
                body.Append("<br>");
                await NotificationService!.EmailNotification("Codis365 SOC-Security Operator Center)", "jbalcellss@uoc.edu", "", "New user logged in: " + userId, body.ToString(), "");
            }
                
            await userRepository.AddUserDD(userDDDTO);

            return true;
        }

        /// <summary>
        /// Get User Device Detector
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserDDDTO GetUserDeviceDetector(Guid? userId)
        {
            try
            {
                DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
                DeviceDetectorSettings.RegexesDirectory = hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString();

                var headers = httpContextAccessor.HttpContext!.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                var userAgent = httpContextAccessor.HttpContext!.Request.Headers.UserAgent;  // ["User-Agent"]
                var clientHints = ClientHints.Factory(headers);
                var dd = new DeviceDetector(userAgent, clientHints);
                dd.SetCache(new DictionaryCache());
                dd.DiscardBotInformation();
                dd.SkipBotDetection();
                dd.Parse();

                userDDDTO.UserId = userId;
                userDDDTO.DDClient = dd.GetClient();
                userDDDTO.DDModel = dd.GetModel();
                userDDDTO.DDBrand = dd.GetBrand();
                userDDDTO.DDBrandName = dd.GetBrandName();
                userDDDTO.DDOs = dd.GetOs();
                userDDDTO.DDBrowser = dd.GetBrowserClient();
                userDDDTO.DDtype = dd.GetDeviceName();
                userDDDTO.DDCity = "<Private IP Address>";
            }
            catch (Exception ex) {
                Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
            }

            // Ip Location
            try
            {
                if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext!.Connection.RemoteIpAddress != null)
                    userDDDTO.IPAddress = httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString(); // Determine the IP Address of the request 
            }
            catch (Exception ex)
            {
                Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
            }

            if (userDDDTO.IPAddress == "127.0.0.1" || userDDDTO.IPAddress == "0.0.0.0" || userDDDTO.IPAddress == "::1") {
                userDDDTO.DDCity = "Local"; 
            }
            else
            {
                try
                {
                    // Get the city from the IP Address
                    try
                    {
                        if (Debugger.IsAttached)
                        {
                            userDDDTO.DDCity= "Local";
                        }
                        else
                        {
                            pathToDB = "/var/www/TFG-GEI_WebApp/PresentationLayer/wwwroot/geoip2/";
                            IpReader = new DatabaseReader(pathToDB + "GeoLite2-City.mmdb");
                            userDDDTO.DDCity = IpReader!.City(userDDDTO.IPAddress!).City.Name + " - " + IpReader.City(userDDDTO.IPAddress!).Country.Name;
                        }

                    } 
                    catch (AddressNotFoundException ex) {
                        Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                        userDDDTO.DDCity = "<Private IP Address>";
                    }
                    catch (InvalidDatabaseException ex) {
                        Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                        userDDDTO.DDCity = "<Private IP Address>";
                    }
                }
                catch (Exception ex) {
                    Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                    userDDDTO.DDCity = "<Private IP Address>";
                }
            }

            return userDDDTO;
        }
    }
}

