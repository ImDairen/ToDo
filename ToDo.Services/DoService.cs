using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDo.Data.Interfaces;
using ToDo.Data.Models.Static;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using System.Linq;
using ToDo.Data.Models;
using System;

namespace ToDo.Services
{
    public class DoService : IDoService
    {
        IUnitOfWork Data { get; set; }

        public DoService(IUnitOfWork unitOfWork)
        {
            Data = unitOfWork;
        }

        public DoServiceModel GetDo(int id)
        {
            var item = Data.ToDoes.Get(id);

            if (item == null)
                throw new ValidationException("Задача не найдена");

            return new DoServiceModel(item);
        }

        public void CreateDo(DoServiceModel model)
        {
            var newDo = new Do
            {
                Title = model.Title,
                Description = model.Description,
                Executors = model.Executors,
                Status = DoStatus.Created,
                Created = DateTime.Now,
                Plan = model.Plan
            };

            Data.ToDoes.Create(newDo);
            Data.Save();
        }

        public void UpdateDo(DoServiceModel model)
        {
            var toDo = Data.ToDoes.Get(model.Id);

            if (toDo == null)
                throw new ValidationException("Задача не найдена");

            Data.ToDoes.Update(toDo);
        }

        public void DeleteDo(int id)
        {
            var toDo = Data.ToDoes.Get(id);

            if (toDo == null)
                throw new ValidationException("Задача не найдена");

            if (toDo.SubTasks.Count > 0)
                foreach (var item in toDo.SubTasks)
                {
                    Data.ToDoes.Delete(item.Id);
                }

            Data.ToDoes.Delete(toDo.Id);
        }

        public IEnumerable<DoServiceModel> GetDoes()
        {
            var subTasks = Data.ToDoes.GetAll()
                .Select(x => x.SubTasks).SelectMany(x => x).ToList().Distinct();

            var toDoes = Data.ToDoes.GetAll()
                .Where(x => !subTasks.Any(sub => sub.Id == x.Id))
                .Select(x => new DoServiceModel(x));

            return toDoes;
        }

        public void ChangeDoStatus(int id, DoStatus status)
        {
            var toDo = Data.ToDoes.Get(id);

            if (toDo == null)
                throw new ValidationException("Задача не найдена");

            if (!CompleteSubTasks(toDo))
                throw new ValidationException("Одна из подзадач этой задачи не может быть завершена");
            
            toDo.Status = DoStatus.Done;
        }

        private bool CompleteSubTasks(Do entity)
        {
            foreach (var item in entity.SubTasks)
                CompleteDo(item);

            return true;
        }

        private void CompleteDo(Do entity)
        {
            if (entity.Status == DoStatus.Created)
                throw new ValidationException("Задача не может быть завершена, так как не была в процессе выполнения");
            else if (entity.Status == DoStatus.Paused)
                throw new ValidationException("Задача не может быть завершена, так как ее выполнение было приостановлено");
            else if (entity.Status == DoStatus.Processing)
                entity.Status = DoStatus.Done;
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
}
