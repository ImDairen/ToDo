using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ToDo.Data.Models.Static;
using ToDo.Services.Models;

namespace ToDo.Web.Models.Do
{
    public class DoListingViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual List<DoListingViewModel> SubTasks { get; set; }


        public DoListingViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;

            if (model.SubTasks != null && model.SubTasks.Any())
            {
                SubTasks = model.SubTasks.Select(s => new DoListingViewModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoListingViewModel>();
            }
        }
    }

    public class DoCreateViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public int Plan { get; set; }
    }

    public class DoUpdateViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public DoStatus Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime? Done { get; set; }

        public int? Fact { get; set; }

        public DoUpdateViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            Executors = model.Executors;
            Status = model.Status;
            Done = model.Done;
            Fact = model.Fact;
        }

        public DoUpdateViewModel()
        {

        }
    }

    public class DoDescriptionViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DoDescriptionViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Description = model.Description;
        }
    }

    public class DoDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public DoStatus Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime? Done { get; set; }

        public int Plan { get; set; }

        public int? Fact { get; set; }


        public List<DoDetailsViewModel> SubTasks { get; set; }

        public DoDetailsViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            Executors = model.Executors;
            Status = model.Status;
            Created = model.Created;
            Done = model.Done;
            Plan = model.Plan;
            Fact = model.Fact;

            if (model.SubTasks != null && model.SubTasks.Any())
            {
                SubTasks = model.SubTasks.Select(s => new DoDetailsViewModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoDetailsViewModel>();
            }
        }
    }

    public class DoDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DoDeleteViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
        }
    }

}
