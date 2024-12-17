namespace ExternalAPI.Helpers
{
    /// <summary>
    /// Helper class for system utilities.
    /// </summary>
    /// <param name="HttpContextAccessor"></param>
    public class SystemUtilsHelper(IHttpContextAccessor HttpContextAccessor) : ISystemUtilsHelper
    {
        /// <summary>
        /// Get the username of the current user.
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        {
            string result = "";
            var user = HttpContextAccessor.HttpContext?.User;
            if (user != null || user!.Identity!.IsAuthenticated)
                result = user.Identity!.Name!;

            return result;
        }
    }
}
