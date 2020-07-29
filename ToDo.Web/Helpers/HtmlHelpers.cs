using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Data.Models.Static;

namespace ToDo.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsSelected(
            this IHtmlHelper htmlHelper,
            string controllers,
            string actions,
            string cssClass = "nav active"
            )
        {
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

        public static object PropertyValue(
            this IHtmlHelper htmlHelper,
            object value
            )
        {
            object currentValue = htmlHelper.ViewContext.HttpContext.Items.Equals(value);

            if (value is string && value != null)
            {
                return value;
            }
            else if (value is int && value != null)
            {
                return value;
            }
            else if (value is DateTime && value != null)
            {
                return DateTime.Now;
            }
            else if (value is DoStatus && value != null)
            {
                return value;
            }
            else return null;
        }
    }
}
