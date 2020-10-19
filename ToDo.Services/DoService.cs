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

            if (model.Status == DoStatus.Done)
                CompleteTerminalTask(toDo);
            else
                toDo.Status = model.Status;

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

            if (toDo.SubTasks != null && toDo.SubTasks.Any())
            {
                toDo.SubTasks = null;
                Data.ToDoes.Update(toDo);
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

            #region Another way

            //var subTasks = Data.ToDoes.GetAll()
            //    .Select(x => x.SubTasks)
            //    .SelectMany(x => x)
            //    .ToList()
            //    .Distinct();

            //var toDoes = Data.ToDoes.GetAll()
            //    .Where(x => !subTasks
            //        .Any(sub => sub.Id == x.Id))
            //    .Select(x => new DoServiceModel(x));

            #endregion

            return toDoes;
        }

        private void CompleteTerminalTask(Do entity)
        {
            if (CheckSubTasksCanBeCompleted(entity))
            {
                ChangeSubTasksDoneStatus(entity);
                entity.Status = DoStatus.Done;
            }
        }

        private void ChangeSubTasksDoneStatus(Do entity)
        {
            foreach (var sub in entity.SubTasks)
                sub.Status = DoStatus.Done;
        }

        private bool CheckSubTasksCanBeCompleted(Do entity)
        {
            foreach (var sub in entity.SubTasks)
            {
                if (sub.Status != DoStatus.Done && sub.Status != DoStatus.Processing)
                    return false;
            }

            return true;
        }
    }
}
