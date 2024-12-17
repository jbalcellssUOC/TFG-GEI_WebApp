using System.Text;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for helpers operations
    /// </summary>
    public interface IHelpersService
    {
        /// <summary>
        /// Generates a body for create a new password mail
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="TokenURL"></param>
        /// <returns></returns>
        public StringBuilder EmailBodyPassword_New(string Username, string TokenURL);

        /// <summary>
        /// Generates a body for change password mail
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public StringBuilder EmailBodyPassword_Change(string Username);

        /// <summary>
        /// Generates a body for reset password mail
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public StringBuilder EmailBodyAccount_Welcome(string Username, string Name);
    }
}
