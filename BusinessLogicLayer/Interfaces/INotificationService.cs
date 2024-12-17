using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for notification operations
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Email notification service
        /// </summary>
        /// <param name="ParEmp"></param>
        /// <param name="ParamTo"></param>
        /// <param name="ParamBcc"></param>
        /// <param name="ParamSubject"></param>
        /// <param name="ParamBody"></param>
        /// <param name="Attachments"></param>
        /// <returns></returns>
        public Task<IActionResult> EmailNotification(string ParEmp, string ParamTo, string ParamBcc, string ParamSubject, string ParamBody, string Attachments);
    }
}
