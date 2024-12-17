using Asp.Versioning;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Entities.POCO;
using ExternalAPI.POCO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// Represents the main entry point for API requests in the application. This controller
    /// handles various actions related to [specific domain - e.g., user management, data processing].
    /// </summary>
    /// <remarks>
    /// This controller is designed to serve as a central hub for the application's primary functionalities.
    /// It includes methods for authenticating users and managing session states.
    /// Ensure that all methods are properly secured and accessible only to authorized users where applicable.
    /// </remarks>

    [ApiVersion(1.0)]
    [ApiController]
    [Route("v1/[controller]")]

    public class AuthenticationController(
        IConfiguration configuration,
        IAuthService authService,
        IUserService userService,
        IProductRepository productRepository,
        IBarcodeRepository barcodeRepository
        ) : ControllerBase
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConfiguration Configuration = configuration;
        private readonly IAuthService AuthService = authService;
        private readonly IUserService UserService = userService;
        private readonly IProductRepository ProductRepository = productRepository;
        private readonly IBarcodeRepository BarcodeRepository = barcodeRepository;

        /******************************************************************************/
        /**************** PUBLIC METHODS **********************************************/
        /******************************************************************************/

        /// <summary>
        /// User Authentication and JWT Bearer Token Generation.
        /// </summary>
        /// <param name="userParam"></param>
        /// <param password="userParam"></param>
        /// <returns>A newly created authenthication token.</returns>
        /// <response code="200">Succesfully. Returns a newly created authenthication token.</response>
        /// <response code="400">Bad request. Error in the request parameters.</response>
        /// <response code="401">Unauthorized. You are not authorized to use this method.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="429">Too Many Requests. The application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "This POST method will allow you to authenticate your user code (your Codis365 E-mail) with the same access data that you currently have on the Codis365 portal in order to obtain the JWT Bear Token. Then in order to use the Codis365 API methods you will need to use JWT Bearer authentication with your own token. The maximum duration of a token is 15 minutes, during which you can carry out all the operations you need to apply in Codis365 API. Once the token has expired you must request a new one if you want to continue using the Codis365 API otherwise you will receive 401 Http error message (Unauthorized)." +
            "<br /><br />Calls to the Codis365 RESTful API are governed by request-based limits, which means you should consider the total number of API calls your app makes. In addition, there are resource-based rate limits and throttles. Limits are calculated using the leaky bucket algorithm. All requests that are made after rate limits have been exceeded are throttled and an HTTP 429 status code (Too Many Requests) error is returned. Requests succeed again after enough requests have emptied out of the bucket. You can see the current state of the throttle for a store by using the rate limits header." +
            "<br /><br /><i>Consider the rate of transactions per second for each Ip address and user as follows:</i>  <br /> <br />" +
            "<span id=\"especialfont\">The bucket's unit of measurement is the elapsed time in seconds that it takes to complete a request, then the bucket size is 60 seconds, and this can't be exceeded at any given time or a throttling error is returned. The bucket empties at a leak rate of one per second. The 60-second limit applies to the Ip address of the user interacting with the API. Every request to the Codis365 API costs at least 0.7 seconds to run. After a request completes, the total elapsed time is calculated and subtracted from the bucket.</span><br /> <br />" +
            "<i>Bucket limit example:</i><br /> <br />" +
            "<span id=\"especialfont\">Suppose that an API user requests to a method only take 0.7 seconds or less. Each request would cost the minimum 0.7 seconds. In this scenario, it's possible for an API user to make a maximum of 85 parallel requests while remaining within the 60 second bucket limit (85=60/0.7).</span><br /> <br />" +
            "<p id=\"headerBorder\"><strong>Your API keys and token carry many privileges, so be sure to keep them secret!. Do not share your API keys or token in publicly accessible areas such GitHub, client-side code, and so forth. All API requests must be made over HTTPS and Content-type as application/json thus calls made over plain HTTP will fail with HTTP 405 status code (Method Not Allowed).</strong></p>"
            ,Tags = ["Authenticate Methods"])]

        [SwaggerResponse(StatusCodes.Status200OK, "The token has been successfully obtained.", typeof(ApiResponseAuth))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ObjectResult> Authenticate([FromForm] ApiUserAuth userParam)
        {
            try
            {
                LoginUserDTO loginUserDTO = new()
                {
                    Username = userParam.Username,
                    Password = userParam.Password,
                };

                if (await AuthService.CheckUserAuth(loginUserDTO))
                {
                    var userDetailsDTO = await UserService.GetUserDetails(loginUserDTO.Username!);
                    var ApiKeySecret = Configuration["APIKeySettings:Secret"]!;
                    string securityToken = await AuthService.CreateJWTSecurityToken(loginUserDTO.Username!, ApiKeySecret);
                    if (string.IsNullOrEmpty(securityToken))
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, "Bad Request: The parameters provided in the request are invalid or incomplete.");
                    }

                    var response = new ApiResponseAuth(
                        userDetailsDTO.Login!,
                        userDetailsDTO.Name!,
                        userDetailsDTO.Host!,
                        userDetailsDTO.IPv4!,
                        userDetailsDTO.Location!,
                        "The JWT bearer token has been successfully retrieved and is ready for use in API requests.",
                        tokenExpirationTime: DateTime.Now.AddMinutes(15),
                        securityToken);
                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized: The provided username or password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet("authenticate")]
        public IActionResult Authenticate()
        {
            return StatusCode((int)HttpStatusCode.MethodNotAllowed, "The requested method is not allowed for this resource.");
        }
    }
}
