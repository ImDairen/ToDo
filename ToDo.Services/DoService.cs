using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDo.Data.Interfaces;
using ToDo.Data.Models.Static;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using System.Linq;
using ToDo.Data.Models;
using System;
using System.Data;

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

            if (toDo.Status != model.Status)
            {
                ChangeDoStatus(toDo.Id, model.Status);
            }

            toDo.Title = model.Title;
            toDo.Description = model.Description;
            toDo.Executors = model.Executors;
            toDo.Plan = model.Plan;
            toDo.Fact = model.Fact;
            toDo.Done = model.Done;

            Data.ToDoes.Update(toDo);
            Data.Save();
        }

        public void UpdateDo(int id)
        {
            var toDo = Data.ToDoes.Get(id);

            if (toDo == null)
                throw new ValidationException("Задача не найдена");

            Data.ToDoes.Update(toDo);
            Data.Save();
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
            Data.Save();
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

            switch (status)
            {
                case DoStatus.Created:
                    SetCreated(toDo);
                    break;
                case DoStatus.Processing:
                    SetProcessing(toDo);
                    break;
                case DoStatus.Paused:
                    SetPaused(toDo);
                    break;
                case DoStatus.Done:
                    SetDone(toDo);
                    break;
            }
        }

        private void SetCreated(Do entity)
        {
            if (entity.Status != DoStatus.Created)
            {
                throw new ValidationException("Задача не может быть созданной второй раз");
            }

            entity.Status = DoStatus.Created;
            Data.ToDoes.Update(entity);
            Data.Save();
        }

        private void SetProcessing(Do entity)
        {
            if (entity.Status == DoStatus.Done)
            {
                throw new ValidationException("Выполненная задача не может выполняться заного");
            }

            entity.Status = DoStatus.Processing;
            Data.ToDoes.Update(entity);
            Data.Save();
        }

        private void SetPaused(Do entity)
        {
            if (entity.Status != DoStatus.Processing)
            {
                throw new ValidationException("Задача не может быть приостановлена, так как не было начато ее выполнение");
            }

            entity.Status = DoStatus.Paused;
            Data.ToDoes.Update(entity);
            Data.Save();
        }

        private void SetDone(Do entity)
        {
            if (entity.Status == DoStatus.Processing && CheckSubTasksCompleted(entity))
            {
                if (entity.Status == DoStatus.Created)
                    throw new ValidationException("Задача не может быть завершена, так как не была в процессе выполнения");
                else if (entity.Status == DoStatus.Paused)
                    throw new ValidationException("Задача не может быть завершена, так как ее выполнение было приостановлено");
                else if (entity.Status == DoStatus.Processing)
                    entity.Status = DoStatus.Done;

                Data.ToDoes.Update(entity);
                Data.Save();
            }
            else
                throw new ValidationException("Задача не может быть завершена, если она не в процессе выполнения или завершены не все подзадачи");
        }

        private bool CheckSubTasksCompleted(Do entity)
        {
            foreach (var item in entity.SubTasks)
            {
                if (item.Status != DoStatus.Done)
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            Data.Dispose();
        }
    }
}
