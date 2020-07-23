using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ToDo.Services.Interfaces;
using ToDo.Web.Models.Do;

namespace ToDo.Web.Controllers
{
    public class DoController : Controller
    {
        private readonly ILogger<DoController> _logger;
        private readonly IDoService _toDoService;

        public DoController(
            ILogger<DoController> logger,
            IDoService toDoService,
            IHtmlHelper htmlHelper
            )
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        public IActionResult Index()
        {
            var subTasks = _toDoService.GetAll()
                .Select(x => x.SubTasks).SelectMany(x => x).ToList().Distinct();

            var model = _toDoService.GetAll()
                .Where(x => !subTasks.Any(sub => sub.Id == x.Id))
                .Select(x => new DoListingModel(x));

            return View(model);
        }

        public JsonResult GetDescription(string jsonInput)
        {
            string description = _toDoService.GetById(int.Parse(jsonInput)).Description;

            if (description != null)
            {
                return Json(description);
            }
            else
                return Json("This task has no description");
        }

        public JsonResult ExpandItem(int id)
        {
            var item = _toDoService.GetById(id);

            if (item != null && item.SubTasks != null)
            {
                return Json(item.SubTasks);
            }

            return Json("");
        }
    }
}
