namespace ExternalAPI.Helpers
{
    /// <summary>
    /// SystemUtilsHelper class
    /// </summary>
    public interface ISystemUtilsHelper
    {
        /// <summary>
        /// Get the username of the authenticated user.
        /// </summary>
        /// <returns></returns>
        public string GetUsername();
    }
}
