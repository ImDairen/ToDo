using System.Collections.Generic;
using ToDo.Data.Interfaces;
using ToDo.Data.Models.Static;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;
using System.Linq;
using ToDo.Data.Models;
using System;
using System.Data;
using ToDo.Services.Infrastructure.Exceptions;

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
            var item = Data.ToDoes.FindByIdAsync(id).Result;

            if (item == null)
                throw new NotFindException("GetDo");

            return new DoServiceModel(item);
        }

        public int? CreateDo(DoServiceModel model)
        {
            try
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

                Data.ToDoes.Insert(newDo);
                Data.Save();
                return newDo.Id;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public void UpdateDo(DoServiceModel model)
        {
            var toDo = Data.ToDoes.FindByIdAsync(model.Id).Result;

            if (toDo == null)
                throw new NotFindException("UpdateDo");

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

            if (model.SubTasks != null)
            {
                var subIds = model.SubTasks.Select(x => x.Id);
                var subs = Data.ToDoes.AsQueryable()
                    .Where(x => subIds.Contains(x.Id));

                foreach (var item in subs)
                {
                    toDo.SubTasks.Add(item);
                }
            }
            

            Data.ToDoes.Update(toDo);
            Data.Save();
        }

        public void DeleteDo(int id)
        {
            var toDo = Data.ToDoes.FindByIdAsync(id).Result;

            if (toDo == null)
                throw new NotFindException("DeleteDo");

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
            var toDoes = Data.ToDoes
                .GetAllAsync().Result
                .Where(x => 
                    !Data.ToDoes.GetAllAsync().Result
                    .Select(x => x.SubTasks)
                    .SelectMany(x => x)
                    .Contains(x))
                .Select(x => new DoServiceModel(x));

            //var subTasks = Data.ToDoes.GetAll()
            //    .Select(x => x.SubTasks)
            //    .SelectMany(x => x)
            //    .ToList()
            //    .Distinct();

            //var toDoes = Data.ToDoes.GetAll()
            //    .Where(x => !subTasks
            //        .Any(sub => sub.Id == x.Id))
            //    .Select(x => new DoServiceModel(x));

            return toDoes;
        }

        public void ChangeDoStatus(int id, DoStatus status)
        {
            var toDo = Data.ToDoes.FindByIdAsync(id).Result;

            if (toDo == null)
                throw new NotFindException("ChangeDoStatus");

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
                throw new DoSetStatusException(DoStatus.Created, entity);

            entity.Status = DoStatus.Created;
            Data.ToDoes.Update(entity);
            Data.Save();
        }

        private void SetProcessing(Do entity)
        {
            if (entity.Status == DoStatus.Done)
                throw new DoSetStatusException(DoStatus.Done, entity);

            entity.Status = DoStatus.Processing;
            Data.ToDoes.Update(entity);
            Data.Save();
        }

        private void SetPaused(Do entity)
        {
            if (entity.Status != DoStatus.Processing)
            {
                throw new DoSetStatusException(DoStatus.Processing, entity);
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
                    throw new DoSetDoneException();
                else if (entity.Status == DoStatus.Paused)
                    throw new DoSetDoneException();
                else if (entity.Status == DoStatus.Processing)
                    entity.Status = DoStatus.Done;

                Data.ToDoes.Update(entity);
                Data.Save();
            }
            else
                throw new DoSetDoneException();
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
    }
}
