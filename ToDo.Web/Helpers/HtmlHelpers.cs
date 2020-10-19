using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Data.Models.Static;
using ToDo.Web.Models.Do;

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

        public static IEnumerable<SelectListItem> updateModelSelectListItem(
            this IHtmlHelper htmlHelper,
            DoUpdateViewModel model)
        {
            var statusList = new List<SelectListItem>();

            switch (model.Status)
            {
                case DoStatus.Created:
                    statusList.Add(new SelectListItem { Text = DoStatus.Created.ToString()});
                    statusList.Add(new SelectListItem { Text = DoStatus.Processing.ToString() });
                    break;
                case DoStatus.Processing:
                    statusList.Add(new SelectListItem { Text = DoStatus.Processing.ToString() });
                    statusList.Add(new SelectListItem { Text = DoStatus.Paused.ToString() });
                    statusList.Add(new SelectListItem { Text = DoStatus.Done.ToString() });
                    break;
                case DoStatus.Paused:
                    statusList.Add(new SelectListItem { Text = DoStatus.Processing.ToString() });
                    statusList.Add(new SelectListItem { Text = DoStatus.Paused.ToString() });
                    break;
                case DoStatus.Done:
                    statusList.Add(new SelectListItem { Text = DoStatus.Done.ToString() });
                    break;
                default:
                    break;
            }

            return statusList;
        }
    }
}
