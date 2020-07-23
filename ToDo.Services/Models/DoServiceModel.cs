using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ToDo.Data.Models;
using ToDo.Data.Models.Static;

namespace ToDo.Services.Models
{
    public class DoServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public ToDoStatus Status { get; set; }

        public DateTime Done { get; set; }

        private int _plan { get; set; }

        [Required]
        public int Plan
        {
            get
            {
                if (SubTasks == null)
                    return _plan;
                else
                    return _plan + SubTasks.Sum(x => x.Plan);
            }

            set
            {
                _plan = value;
            }
        }

        private int _fact { get; set; }

        [Required]
        public int Fact
        {
            get
            {
                if (SubTasks == null)
                    return _fact;
                else
                    return _fact + SubTasks.Sum(x => x.Fact);
            }

            set
            {
                _fact = value;
            }
        }


        public virtual IEnumerable<DoServiceModel> SubTasks { get; set; }

        public DoServiceModel(Do entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Executors = entity.Executors;
            Created = entity.Created;
            Done = entity.Done;
            Status = entity.Status;
            Plan = entity.Plan;
            Fact = entity.Fact;

            if (entity.SubTasks != null && entity.SubTasks.Any())
            {
                SubTasks = entity.SubTasks.Select(s => new DoServiceModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoServiceModel>();
            }
        }
    }
}
