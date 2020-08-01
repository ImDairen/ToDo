﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using ToDo.Web.Models.Do;

namespace ToDo.Web.Controllers
{
    public class DoController : Controller
    {
        private readonly ILogger<DoController> _logger;
        private readonly IDoService _doService;

        public DoController(
            ILogger<DoController> logger,
            IDoService doService
            )
        {
            _logger = logger;
            _doService = doService;
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
                    Plan = model.Plan
                };

                _doService.CreateDo(newDo);

                TempData["Message"] = "Задача " + newDo.Title + " успешно создана!";

                return RedirectToAction("Index");
            }
            
            TempData["Message"] = "Задача " + model.Title + " не может быть создана!";

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
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

            if (toDo == null)
                throw new ValidationException("Терминальная задача не найдена");

            ViewData["TerminalId"] = toDo.Id;
            ViewData["TerminalTitle"] = toDo.Title;
            
            return View();
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
                    Plan = model.Plan
                };

                _doService.CreateDo(newDo);

                terminal.SubTasks.Add(newDo);
                _doService.UpdateDo(terminal);

                TempData["Message"] = "Подзадача " + newDo.Title + " успешно создана!";

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Подзадача " + model.Title + " не может быть создана!";

            return RedirectToAction("Index");
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
