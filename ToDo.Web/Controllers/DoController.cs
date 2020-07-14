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
            var toDoes = _toDoService.GetAll()
                .Where(x => x.SubTasks.Count != 0)
                .Select(x => new DoListingModel(x));

            var model = new DoIndexModel(toDoes);

            return View(model);
        }
    }
}
