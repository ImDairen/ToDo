using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using ToDo.Web.Models.Do;

namespace ToDo.Web.Controllers
{
    public class DoController : Controller
    {
        private readonly ILogger<DoController> _logger;
        private readonly IDoService _toDoService;

        public DoController(
            ILogger<DoController> logger,
            IDoService toDoService
            )
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        public IActionResult Index()
        {
            var model = _toDoService.GetDoes().Select(x => new DoListingModel(x));

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateNewToDo(DoServiceModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //        }
        //        catch (Exception exception)
        //        {

        //            throw;
        //        }
        //    }
        //    await _toDoService.Create(model);

        //    return View(model);
        //}

        public JsonResult GetDescription(string jsonInput)
        {
            string description = _toDoService.GetDo(int.Parse(jsonInput)).Description;

            if (description != null)
            {
                return Json(description);
            }
            else
                return Json("This task has no description");
        }
    }
}
