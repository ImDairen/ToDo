using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ToDo.Services.Infrastructure.Exceptions;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using ToDo.Web.Infrastructure.Logger;
using ToDo.Web.Models.Do;

namespace ToDo.Web.Controllers
{
    [SkipStatusCodePages]
    public class DoController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IDoService _doService;
        private readonly IStringLocalizer<DoController> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public DoController(
            ILoggerManager logger,
            IDoService doService,
            IStringLocalizer<DoController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
            )
        {
            _logger = logger;
            _doService = doService;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            var model = _doService.GetDoes()
                .Select(x => new DoListingViewModel(x));
            
            return View(model);
        }

        public ViewResult Create()
        {
            return View(new DoCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(
            [Bind("Title, Description, Executors, Plan")] DoCreateViewModel model
            )
        {
            if (ModelState.IsValid)
            {
                var newDo = new DoServiceModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    Plan = int.Parse(model.Plan)
                };

                _doService.CreateDo(newDo);

                TempData["Message"] = "Задача " + newDo.Title + " успешно создана!";

                return RedirectToAction("Index");
            }
            
            TempData["Message"] = "Задача " + model.Title + " не может быть создана!";

            return View(model);
        }

        public ViewResult Update(int id)
        {
            var toDo = _doService.GetDo(id);

            if (toDo != null)
            {
                return View(new DoUpdateViewModel(toDo));
            }

            throw new NullReferenceException(message: "Задача не может быть изменена, так как не была найдена в БД");
        }

        [HttpPost]
        public IActionResult Update(
            [Bind("Id, Title, Description, Executors, Status, Done, Fact")] DoUpdateViewModel model
            )
        {
            if (ModelState.IsValid)
            {
                var doForUpdate = _doService.GetDo(model.Id);

                if (doForUpdate != null)
                {
                    var updatingDo = new DoServiceModel
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Description = model.Description,
                        Executors = model.Executors,
                        Status = model.Status,
                        Done = model.Done,
                        Fact = model.Fact
                    };

                    _doService.UpdateDo(updatingDo);
                    
                    TempData["Message"] = "Задача " + model.Title + " успешно обновлена!";
                }

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Задача " + model.Title + " не может быть обновлена";

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _doService.GetDo(id);

            if (model != null)
                return View(new DoDetailsViewModel(model));
            else
                return NotFound();
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var toDo = _doService.GetDo(id);

            if (toDo != null)
            {
                return View(new DoDeleteViewModel(toDo));
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var toDo = _doService.GetDo(id);

            if (toDo != null)
            {
                _doService.DeleteDo(toDo.Id);

                TempData["Message"] = "Задача " + toDo.Title + " была успешно удалена";

                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        public ViewResult AddSubTask(int id)
        {
            var toDo = _doService.GetDo(id);

            ViewData["TerminalId"] = toDo.Id;
            ViewData["TerminalTitle"] = toDo.Title;
            
            return View(new DoCreateViewModel());
        }

        [HttpPost]
        public IActionResult AddSubTask(int terminalId,
            [Bind("Title, Description, Executors, Plan")] DoCreateViewModel model
            )
        {
            var terminal = _doService.GetDo(terminalId);

            if (terminal == null)
                throw new ValidationException("Терминальная задача не найдена");

            if (ModelState.IsValid)
            {
                var newDo = new DoServiceModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    Plan = int.Parse(model.Plan)
                };

                newDo = _doService.GetDo(_doService.CreateDo(newDo).Value);

                terminal.SubTasks.Add(newDo);
                _doService.UpdateDo(terminal);

                TempData["Message"] = "Подзадача " + newDo.Title + " успешно создана!";

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Подзадача " + model.Title + " не может быть создана!";

            return RedirectToAction("Index");
        }

        public IActionResult ThrowException()
        {
            throw new DoSetDoneException();
        }

        public string GetCulture(string code="")
        {
            if (!String.IsNullOrEmpty(code))
            {
                CultureInfo.CurrentCulture = new CultureInfo(code);
                CultureInfo.CurrentUICulture = new CultureInfo(code);
            }

            return $"CurrentCulture: { CultureInfo.CurrentCulture.Name}, CurrentUICulture: { CultureInfo.CurrentUICulture.Name}";
            
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public PartialViewResult GetDescription(string jsonInput)
        {
            var model = new DoDescriptionViewModel(_doService.GetDo(int.Parse(jsonInput)));

            if (model != null)
            {
                return PartialView("_DisplayTaskDescriptionPartial", model);
            }
            else
                throw new NullReferenceException();
        }
    }
}
