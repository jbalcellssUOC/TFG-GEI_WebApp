using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Helpers
{
    /// <summary>
    /// HtmlHelpers class to provide helper methods for HTML.
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// IsSelected method to check if the current controller and action match the provided controller and action.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string IsSelected(this IHtmlHelper html, string? controller = null, string? action = null, string? cssClass = null)
        {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"]!;
            string currentController = (string)html.ViewContext.RouteData.Values["controller"]!;

            if (string.IsNullOrEmpty(controller))
                controller = currentController!;

            if (string.IsNullOrEmpty(action))
                action = currentAction!;

            return controller == currentController && action == currentAction ?
                cssClass : string.Empty;
        }

        /// <summary>
        /// PageClass method to return the current action as a string.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"]!;
            return currentAction!;
        }
    }
}
